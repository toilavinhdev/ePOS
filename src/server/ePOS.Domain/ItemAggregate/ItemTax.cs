using System.ComponentModel.DataAnnotations.Schema;
using ePOS.Shared.ValueObjects;

namespace ePOS.Domain.ItemAggregate;

public class ItemTax : AuditableEntity
{
    public bool IsTaxIncludePrice { get; set; }
    
    public int Value { get; set; }
    
    public Guid ItemId { get; set; }
    [ForeignKey(nameof(ItemId))] 
    public Item Item { get; set; } = default!;
}