using System.ComponentModel.DataAnnotations;
using ePOS.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace ePOS.Infrastructure.Identity.Aggregate;

public class ApplicationRole : IdentityRole<Guid>, IAuditableEntity
{
    public Guid TenantId { get; set; }

    public long SubId { get; set; }
    
    public RoleStatus Status { get; set; }
    
    [StringLength(256)]
    public string? Description { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public Guid? CreatedBy { get; set; }
    
    public DateTimeOffset? ModifiedAt { get; set; }
    
    public Guid? ModifiedBy { get; set; }
    
    public ICollection<ApplicationUserRole> UserRoles { get; set; } = default!;

    public ICollection<ApplicationRoleClaim> RoleClaims { get; set; } = default!;
}

public enum RoleStatus
{
    Active,
    Lock
}