using System.Text;
using ePOS.Infrastructure.Identity.Aggregate;
using ePOS.Infrastructure.Identity.Authorization;
using ePOS.Infrastructure.Persistence;
using ePOS.Shared.ValueObjects;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace ePOS.API.Configurations;

public static class IdentitySetup
{
    public static IServiceCollection AddIdentitySetup(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddSingleton<IAuthorizationPolicyProvider, ApplicationPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, ApplicationAuthorizationHandler>();
        
        services
            .AddIdentity<ApplicationUser, ApplicationRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<TenantContext>()
            .AddDefaultTokenProviders();
        
        services
            .AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.JwtTokenConfig.ServerSecretKey)),
                    ClockSkew = TimeSpan.Zero
                };
            });
        return services;
    }
}