using ePOS.Infrastructure.MigrateData;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Polly;

namespace ePOS.Infrastructure.Persistence;

public static class TenantContextSeed
{
    public static async Task SeedAsync(TenantContext context, ILogger<TenantContext> logger, IServiceProvider serviceProvider)
    {
        var policy = Policy.Handle<SqlException>().WaitAndRetryAsync(
            retryCount: 5,
            sleepDurationProvider: _ => TimeSpan.FromSeconds(10),
            onRetry: (exception, retryCount) =>
            {
                logger.LogError("Retry {0} - {1} - Exception {2}: {3}", retryCount, 5, exception.GetType().Name, exception.Message);
            });

        await policy.ExecuteAsync(async () =>
        {
            await MigrateUser.SeedUsersAsync(context, serviceProvider);
            await MigrateUnit.SeedUnitsAsync(context);
            await context.SaveChangesAsync();
        });
    }
}