namespace ePOS.Shared.ValueObjects;

public interface IEntity
{
    public Guid Id { get; set; }
    
    public Guid TenantId { get; set; }
    
    public long SubId { get; set; }
}

public interface IAuditableEntity : IEntity
{
    public DateTimeOffset CreatedAt { get; set; }
    
    public Guid? CreatedBy { get; set; }
    
    public DateTimeOffset? ModifiedAt { get; set; }
    
    public Guid? ModifiedBy { get; set; }
}

public class Entity : IEntity
{
    public Guid Id { get; set; }
    
    public Guid TenantId { get; set; }
    
    public long SubId { get; set; }
}

public class AuditableEntity : Entity, IAuditableEntity
{
    public DateTimeOffset CreatedAt { get; set; }
    
    public Guid? CreatedBy { get; set; }
    
    public DateTimeOffset? ModifiedAt { get; set; }
    
    public Guid? ModifiedBy { get; set; }
    
    public virtual void SetCreationTracking(Guid tenantId, Guid? userId)
    {
        CreatedAt = DateTimeOffset.UtcNow;
        TenantId = tenantId;
        CreatedBy = userId;
    }

    public virtual void SetModificationTracking(Guid? userId)
    {
        ModifiedAt = DateTimeOffset.UtcNow;
        ModifiedBy = userId;
    }
    
    public virtual void SetCreationTracking(UserClaimsValue userClaimsValue)
    {
        SetCreationTracking(userClaimsValue.TenantId, userClaimsValue.Id);
    }

    public virtual void SetModificationTracking(UserClaimsValue userClaimsValue)
    {
        SetModificationTracking(userClaimsValue.Id);
    }
}