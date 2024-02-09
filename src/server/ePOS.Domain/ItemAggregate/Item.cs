using System.ComponentModel.DataAnnotations.Schema;
using ePOS.Domain.CategoryAggregate;
using ePOS.Domain.UnitAggregate;
using ePOS.Shared.ValueObjects;

namespace ePOS.Domain.ItemAggregate;

public class Item : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;

    public string Sku { get; set; } = default!;
    
    public Guid UnitId { get; set; }
    [ForeignKey(nameof(UnitId))]
    public Unit Unit { get; set; } = default!;
    
    public double Price { get; set; }

    public ItemTax? ItemTax { get; set; }
    
    public List<ItemImage>? ItemImages { get; set; }
    
    public List<ItemSize>? ItemSizes { get; set; }
    
    public List<ItemOptionAttribute>? ItemOptionAttributes { get; set; }
    
    public List<ItemTopping>? ItemToppings { get; set; }
    
    public List<CategoryItem>? CategoryItems { get; set; }
}