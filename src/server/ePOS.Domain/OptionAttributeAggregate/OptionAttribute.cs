using ePOS.Domain.ItemAggregate;
using ePOS.Shared.ValueObjects;

namespace ePOS.Domain.OptionAttributeAggregate;

public class OptionAttribute : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    
    public bool IsActive { get; set; }
    
    public int SortIndex { get; set; }

    public List<OptionAttributeValue> OptionAttributeValues { get; set; } = default!;
    
    public List<ItemOptionAttribute>? ItemOptionAttributes { get; set; }
}