using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace ePOS.Infrastructure.Identity.Authorization;

public class ApplicationPolicyProvider : IAuthorizationPolicyProvider
{
    private DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

    public ApplicationPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }
    
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return FallbackPolicyProvider.GetDefaultPolicyAsync();
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return FallbackPolicyProvider.GetFallbackPolicyAsync();
    }
    
    public async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        var model = policyName.Split("|");
        
        if (model.Length != 2) return await FallbackPolicyProvider.GetPolicyAsync(policyName);
        
        var policyBuilder = new AuthorizationPolicyBuilder();
        
        policyBuilder.AddRequirements(new ApplicationPolicyRequirement()
        {
            Permission = model[1]
        });
        
        await Task.CompletedTask;

        return policyBuilder.Build();
    }
}