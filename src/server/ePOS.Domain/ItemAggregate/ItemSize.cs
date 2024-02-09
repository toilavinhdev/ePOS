using System.ComponentModel.DataAnnotations.Schema;
using ePOS.Shared.ValueObjects;

namespace ePOS.Domain.ItemAggregate;

public class ItemSize : AuditableEntity
{
    public string Name { get; set; } = default!;
    
    public double Price { get; set; }
    
    public int SortIndex { get; set; }
    
    public Guid ItemId { get; set; }
    [ForeignKey(nameof(ItemId))]
    public Item Item { get; set; } = default!;
}