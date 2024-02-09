using Microsoft.AspNetCore.Authorization;

namespace ePOS.Infrastructure.Identity.Authorization;

public class ApplicationPolicyRequirement : IAuthorizationRequirement
{
    public string Permission { get; set; } = default!;
}