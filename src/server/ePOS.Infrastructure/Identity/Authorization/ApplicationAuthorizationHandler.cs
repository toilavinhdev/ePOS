using ePOS.Infrastructure.Persistence;
using ePOS.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ePOS.Infrastructure.Identity.Authorization;

public class ApplicationAuthorizationHandler : AuthorizationHandler<ApplicationPolicyRequirement>
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ApplicationAuthorizationHandler> _logger;

    public ApplicationAuthorizationHandler(IServiceProvider serviceProvider, ILogger<ApplicationAuthorizationHandler> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ApplicationPolicyRequirement requirement)
    {
        using var scope = _serviceProvider.CreateScope();
        
        var tenantContext = scope.ServiceProvider.GetRequiredService<TenantContext>();
        
        if (context.User.Identity!.IsAuthenticated == false)
        {
            context.Fail();
            throw new UnauthorizedAccessException();
        }
        
        var policy = $"{requirement.Permission}";
        var userId = context.User.Claims.FirstOrDefault(x => x.Type.Equals("id"))?.Value.ToGuid();
        
        var user = await tenantContext.Users
            .Include(x => x.Claims)
            .FirstOrDefaultAsync(x => x.Id.Equals(userId));
        
        if (user is null)
        {
            context.Fail();
            throw new UnauthorizedAccessException();
        }
        
        var hasPolicy = user.Claims.Any(x => x.ClaimValue.Equals(policy) || x.ClaimValue.Equals("All"));
        
        if (hasPolicy == false)
        {
            context.Fail();
            throw new UnauthorizedAccessException();
        }
        
        context.Succeed(requirement);
        
        await Task.CompletedTask;
    }
}