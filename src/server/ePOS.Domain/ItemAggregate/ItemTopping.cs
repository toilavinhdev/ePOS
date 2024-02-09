using System.ComponentModel.DataAnnotations.Schema;
using ePOS.Domain.ToppingAggregate;
using ePOS.Shared.ValueObjects;

namespace ePOS.Domain.ItemAggregate;

public class ItemTopping : Entity
{
    public Guid ItemId { get; set; }
    [ForeignKey(nameof(ItemId))] 
    public Item Item { get; set; } = default!;
    
    public Guid ToppingId { get; set; }
    [ForeignKey(nameof(ToppingId))] 
    public Topping Topping { get; set; } = default!;
}