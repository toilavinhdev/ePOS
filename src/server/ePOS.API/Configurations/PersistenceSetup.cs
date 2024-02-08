using ePOS.Infrastructure.Persistence;
using ePOS.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace ePOS.API.Configurations;

public static class PersistenceSetup
{
    public static IServiceCollection AddPersistenceSetup(this IServiceCollection services, AppSettings appSettings)
    {
        var connectionString = appSettings.ConnectionStrings.SqlServerConnection;
        services.AddDbContext<TenantContext>(o =>
        {
            o.UseSqlServer(connectionString);
        });
        return services;
    }
}