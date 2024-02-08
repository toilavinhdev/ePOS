using Microsoft.AspNetCore.Identity;

namespace ePOS.Infrastructure.Identity.Aggregate;

public class ApplicationUserToken : IdentityUserToken<Guid>
{
    public DateTime Expires { get; set; }

    public virtual ApplicationUser User { get; set; } = default!;
}