using System.ComponentModel.DataAnnotations.Schema;
using ePOS.Domain.ItemAggregate;
using ePOS.Shared.ValueObjects;

namespace ePOS.Domain.OrderAggregate;

public class OrderItem : Entity
{
    public Guid OrderId { get; set; }
    [ForeignKey(nameof(OrderId))] 
    public Order Order { get; set; } = default!;
    
    public Guid ItemId { get; set; }
    [ForeignKey(nameof(ItemId))] 
    public Item Item { get; set; } = default!;
    
    public Guid? ItemSizeId { get; set; }
    [ForeignKey(nameof(ItemSizeId))]
    public ItemSize? ItemSize { get; set; }
    
    public int Quantity { get; set; }
    
    public double TotalLine { get; set; }
}