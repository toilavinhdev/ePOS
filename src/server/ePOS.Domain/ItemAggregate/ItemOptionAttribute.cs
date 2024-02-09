using System.ComponentModel.DataAnnotations.Schema;
using ePOS.Domain.OptionAttributeAggregate;
using ePOS.Shared.ValueObjects;

namespace ePOS.Domain.ItemAggregate;

public class ItemOptionAttribute : Entity
{
    public Guid ItemId { get; set; }
    [ForeignKey(nameof(ItemId))]
    public Item Item { get; set; } = default!;
    
    public Guid OptionAttributeId { get; set; }
    [ForeignKey(nameof(OptionAttributeId))]
    public OptionAttribute OptionAttribute { get; set; } = default!;
    
    public int SortIndex { get; set; }
}