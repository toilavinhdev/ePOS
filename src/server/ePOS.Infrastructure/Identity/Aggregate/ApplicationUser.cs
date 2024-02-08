using System.ComponentModel.DataAnnotations;
using ePOS.Shared.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace ePOS.Infrastructure.Identity.Aggregate;

public class ApplicationUser : IdentityUser<Guid>, IAuditableEntity
{
    public Guid TenantId { get; set; }
    
    public long SubId { get; set; }
    
    [StringLength(256)]
    public string? FullName { get; set; }
    
    public bool IsAdmin { get; set; }
    
    [StringLength(256)]
    public string? AvatarUrl { get; set; }
    
    public UserStatus Status { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public Guid? CreatedBy { get; set; }
    
    public DateTimeOffset? ModifiedAt { get; set; }
    
    public Guid? ModifiedBy { get; set; }
    
    public virtual ICollection<ApplicationUserClaim> Claims { get; set; } = default!;
    
    public virtual ICollection<ApplicationUserLogin> Logins { get; set; } = default!;
    
    public virtual ICollection<ApplicationUserToken> Tokens { get; set; } = default!;
    
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = default!;
}

public enum UserStatus
{
    Active,
    Lock
}