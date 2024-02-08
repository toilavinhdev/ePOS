using ePOS.Application.Common.Contracts;
using ePOS.Infrastructure.Persistence;
using ePOS.Infrastructure.Services;

namespace ePOS.API.Configurations;

public static class ServiceRegistrations
{
    public static IServiceCollection AddServiceRegistrations(this IServiceCollection services)
    {
        services.AddTransient<ITenantContext, TenantContext>();
        services.AddTransient<IUserService, UserService>();
        return services;
    }
}