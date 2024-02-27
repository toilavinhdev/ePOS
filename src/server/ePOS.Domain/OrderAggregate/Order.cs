using ePOS.Shared.ValueObjects;

namespace ePOS.Domain.OrderAggregate;

public class Order : AuditableEntity
{
    public string? Description { get; set; }

    public double SubTotal { get; set; }
    
    public double TotalTax { get; set; }

    public double Total => SubTotal - TotalTax;

    public List<OrderItem> OrderItems { get; set; } = default!;
}