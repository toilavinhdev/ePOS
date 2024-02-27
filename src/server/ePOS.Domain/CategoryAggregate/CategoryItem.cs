using System.ComponentModel.DataAnnotations.Schema;
using ePOS.Domain.ItemAggregate;
using ePOS.Shared.ValueObjects;

namespace ePOS.Domain.CategoryAggregate;

public class CategoryItem
{
    public Guid CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))]
    public Category Category { get; set; } = default!;
    
    public Guid ItemId { get; set; }
    [ForeignKey(nameof(ItemId))]
    public Item Item { get; set; } = default!;
}