using ePOS.Domain.ItemAggregate;
using ePOS.Shared.ValueObjects;

namespace ePOS.Domain.ToppingAggregate;

public class Topping : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    
    public double Price { get; set; }
    
    public bool IsActive { get; set; }
    
    public List<ItemTopping>? ItemToppings { get; set; }
}