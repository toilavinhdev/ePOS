namespace ePOS.Shared.ValueObjects;

public class Entity
{
    public Guid Id { get; set; }
    
    public long SubId { get; set; }
}

public class AuditableEntity : Entity
{
    public DateTimeOffset CreatedAt { get; set; }
    
    public Guid? CreatedBy { get; set; }
    
    public DateTimeOffset? ModifiedAt { get; set; }
    
    public Guid? ModifiedBy { get; set; }
}