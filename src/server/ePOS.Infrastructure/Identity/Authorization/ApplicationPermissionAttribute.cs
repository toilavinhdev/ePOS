using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace ePOS.Infrastructure.Identity.Authorization;

public class ApplicationPermissionAttribute : AuthorizeAttribute
{
    private const string Prefix = "EPOS";

    public string Permission { get; set; } = default!;

    public ApplicationPermissionAttribute()
    {
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
    }
    
    public ApplicationPermissionAttribute(string permission) : this()
    {
        Permission = permission;
        Policy = $"{Prefix}|{Permission}";
    }
}