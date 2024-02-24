using AutoMapper;
using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Application.ViewModels;
using ePOS.Domain.CategoryAggregate;
using ePOS.Domain.ItemAggregate;
using ePOS.Shared.Exceptions;
using ePOS.Shared.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ePOS.Application.Commands;

public class CreateCategoryCommand : IAPIRequest<CategoryViewModel>
{
    public string Name { get; set; } = default!;
    
    public List<Guid>? ItemIds { get; set; }
}

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class CreateCategoryCommandHandler : APIRequestHandler<CreateCategoryCommand, CategoryViewModel>
{
    private readonly ITenantContext _context;
    private readonly IMapper _mapper;
    
    public CreateCategoryCommandHandler(IUserService userService, ITenantContext context, IMapper mapper) : base(userService)
    {
        _context = context;
        _mapper = mapper;
    }

    public override async Task<APIResponse<CategoryViewModel>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category()
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            IsActive = true
        };
        category.SetCreationTracking(UserClaimsValue);
        
        await _context.Categories.AddAsync(category, cancellationToken);

        if (request.ItemIds is not null)
        {
            var categoryItems = new List<CategoryItem>();
            foreach (var itemId in request.ItemIds)
            {
                var itemExisted = await _context.Items.FirstOrDefaultAsync(x => x.Id.Equals(itemId) 
                    && x.TenantId == UserClaimsValue.TenantId, cancellationToken);
                if (itemExisted is null) throw new RecordNotFoundException(nameof(Item), itemId);
                
                categoryItems.Add(new CategoryItem()
                {
                    CategoryId = category.Id,
                    ItemId = itemId
                });
            }
            await _context.CategoryItems.AddRangeAsync(categoryItems, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        var data = _mapper.Map<CategoryViewModel>(category);

        return new APIResponse<CategoryViewModel>().IsSuccess(data, "Tạo danh mục thành công");
    }
}