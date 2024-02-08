using Microsoft.AspNetCore.Identity;

namespace ePOS.Infrastructure.Identity.Aggregate;

public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
{
    public virtual ApplicationRole Role { get; set; } = default!;
}