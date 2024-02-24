using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Domain.CategoryAggregate;
using ePOS.Shared.Exceptions;
using ePOS.Shared.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ePOS.Application.Commands;

public class DeleteCategoryCommand : IAPIRequest
{
    public List<Guid> Ids { get; set; } = default!;
}

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    public DeleteCategoryCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().Must(x => x.Any());
    }
}

public class DeleteCategoryCommandHandler : APIRequestHandler<DeleteCategoryCommand>
{
    private readonly ITenantContext _context;
    
    public DeleteCategoryCommandHandler(IUserService userService, ITenantContext context) : base(userService)
    {
        _context = context;
    }

    public override async Task<APIResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var categories = new List<Category>();

        foreach (var id in request.Ids)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(
                x => x.Id == id && x.TenantId == UserClaimsValue.TenantId, cancellationToken);

            if (category is null) throw new RecordNotFoundException(nameof(Category), id);
            
            categories.Add(category);
        }

        _context.Categories.RemoveRange(categories);
        await _context.SaveChangesAsync(cancellationToken);

        return new APIResponse().IsSuccess($"Xóa thành công {categories.Count} danh mục");
    }
}