using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Domain.CategoryAggregate;
using ePOS.Domain.ItemAggregate;
using ePOS.Shared.Exceptions;
using ePOS.Shared.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ePOS.Application.Commands;

public class CreateCategoryCommand : IAPIRequest
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

public class CreateCategoryCommandHandler : APIRequestHandler<CreateCategoryCommand>
{
    private readonly ITenantContext _context;
    
    public CreateCategoryCommandHandler(IUserService userService, ITenantContext context) : base(userService)
    {
        _context = context;
    }

    public override async Task<APIResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
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
                var itemExisted = await _context.Items.FirstOrDefaultAsync(x => x.Id.Equals(itemId), cancellationToken);
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

        return new APIResponse().IsSuccess("Tạo danh mục thành công");
    }
}