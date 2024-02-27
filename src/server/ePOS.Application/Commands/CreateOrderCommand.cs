using ePOS.Application.Common.Contracts;
using ePOS.Application.Common.Mediator;
using ePOS.Application.ViewModels;
using ePOS.Domain.ItemAggregate;
using ePOS.Domain.OrderAggregate;
using ePOS.Shared.Exceptions;
using ePOS.Shared.ValueObjects;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace ePOS.Application.Commands;

public class CreateOrderCommand : IAPIRequest<OrderViewModel>
{
    public string? Description { get; set; }
    
    public List<OrderItemCreateModel> OrderItems { get; set; } = default!;
}

public class OrderItemCreateModel
{
    public Guid ItemId { get; set; }
    
    public Guid? ItemSizeId { get; set; }
    
    public int Quantity { get; set; }
}

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.OrderItems).NotEmpty();
    }
}

public class CreateOrderCommandHandler : APIRequestHandler<CreateOrderCommand, OrderViewModel>
{
    private readonly ITenantContext _context;
    
    public CreateOrderCommandHandler(IUserService userService, ITenantContext context) : base(userService)
    {
        _context = context;
    }

    public override async Task<APIResponse<OrderViewModel>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new Order()
        {
            Id = Guid.NewGuid(),
            Description = request.Description
        };
        order.SetCreationTracking(UserClaimsValue);
        await _context.Orders.AddAsync(order, cancellationToken);

        double orderSubTotal = 0;
        double orderTotalTax = 0;
        
        var orderItems = new List<OrderItem>();
        
        foreach (var model in request.OrderItems)
        {
            var item = await _context.Items
                .Include(x => x.ItemSizes)
                .FirstOrDefaultAsync(x => x.Id == model.ItemId && x.TenantId == UserClaimsValue.TenantId, cancellationToken);
            if (item is null) throw new RecordNotFoundException(nameof(Item), model.ItemId);

            var size = model.ItemSizeId is not null && item.ItemSizes!.Count > 0
                ? item.ItemSizes.FirstOrDefault(x => x.Id == model.ItemSizeId && x.Item.Id == model.ItemId)
                : null;
            if (model.ItemSizeId is not null && size is null) throw new RecordNotFoundException(nameof(ItemSize), model.ItemSizeId.Value);

            var totalLine = (item.Price + (size?.Price ?? 0)) * model.Quantity;

            orderSubTotal += totalLine;
            orderTotalTax += 0;
            
            var orderItem = new OrderItem()
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ItemId = item.Id,
                ItemSizeId = size?.Id,
                Quantity = model.Quantity,
                TotalLine = totalLine,
                TenantId = UserClaimsValue.TenantId
            };
            
            orderItems.Add(orderItem);
        }
        await _context.OrderItems.AddRangeAsync(orderItems, cancellationToken);

        order.SubTotal = orderSubTotal;
        order.TotalTax = orderTotalTax;
        
        await _context.SaveChangesAsync(cancellationToken);

        var data = new OrderViewModel()
        {
            Id = order.Id,
            Description = order.Description,
            SubTotal = order.SubTotal,
            TotalTax = orderTotalTax,
            OrderItems = orderItems.Select(x => new OrderItemViewModel()
            {
                ItemId = x.ItemId,
                Quantity = x.Quantity,
                TotalLine = x.TotalLine,
                ItemName = x.Item.Name,
                SizeName = x.ItemSize?.Name,
            }).ToList()
        };

        return new APIResponse<OrderViewModel>().IsSuccess(data, "Tạo đơn thành công");
    }
}