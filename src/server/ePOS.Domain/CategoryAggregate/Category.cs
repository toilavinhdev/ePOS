using ePOS.Shared.ValueObjects;

namespace ePOS.Domain.CategoryAggregate;

public class Category : AuditableEntity, IAggregateRoot
{
    public string Name { get; set; } = default!;
    
    public bool IsActive { get; set; }
    
    public List<CategoryItem>? CategoryItems { get; set; }
}