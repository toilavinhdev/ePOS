using ePOS.Domain.ItemAggregate;
using ePOS.Shared.ValueObjects;

namespace ePOS.Domain.UnitAggregate;

public class Unit : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    
    public bool IsDefault { get; set; }
    
    public List<Item>? Items { get; set; }
}