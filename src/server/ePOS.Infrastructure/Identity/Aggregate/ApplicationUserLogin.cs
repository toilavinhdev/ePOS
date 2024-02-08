using Microsoft.AspNetCore.Identity;

namespace ePOS.Infrastructure.Identity.Aggregate;

public class ApplicationUserLogin : IdentityUserLogin<Guid>
{
    public virtual ApplicationUser User { get; set; } = default!;
}