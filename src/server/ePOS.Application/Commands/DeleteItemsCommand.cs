using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Domain.ItemAggregate;
using ePOS.Shared.Exceptions;
using ePOS.Shared.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ePOS.Application.Commands;

public class DeleteItemsCommand : IAPIRequest
{
    public List<Guid> Ids { get; set; } = default!;
}

public class DeleteItemsCommandValidator : AbstractValidator<DeleteItemsCommand>
{
    public DeleteItemsCommandValidator()
    {
        RuleFor(x => x.Ids).NotEmpty().Must(x => x.Any());
    }
}

public class DeleteItemsCommandHandler : APIRequestHandler<DeleteItemsCommand>
{
    private readonly ITenantContext _context;
    
    public DeleteItemsCommandHandler(IUserService userService, ITenantContext context) : base(userService)
    {
        _context = context;
    }

    public override async Task<APIResponse> Handle(DeleteItemsCommand request, CancellationToken cancellationToken)
    {
        var items = new List<Item>();

        foreach (var id in request.Ids)
        {
            var item = await _context.Items.FirstOrDefaultAsync(
                x => x.Id == id && x.TenantId == UserClaimsValue.TenantId, cancellationToken);

            if (item is null) throw new RecordNotFoundException(nameof(Item), id);
            
            items.Add(item);
        }

        _context.Items.RemoveRange(items);
        await _context.SaveChangesAsync(cancellationToken);

        return new APIResponse().IsSuccess($"Xóa thành công {items.Count} món ăn");
    }
}