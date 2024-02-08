using ePOS.Infrastructure.Persistence;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace ePOS.API.Configurations;

public static class MigrateDatabaseSetup
{
    public static WebApplication MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<TenantContext>();
        var logger = serviceProvider.GetRequiredService<ILogger<TenantContext>>();

        try
        {
            logger.LogInformation("Migrating database associated");
            var retry = Policy.Handle<SqlException>().WaitAndRetry(
                retryCount: 5,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(1, retryAttempt)),
                onRetry: (exception, retryCount) =>
                {
                    logger.LogError("Retry {0} - {1} - Exception {2}: {3}", retryCount, 5, exception.GetType().Name, exception.Message);
                });
            
            context.Database.EnsureCreated();

            retry.Execute(() =>
            {
                context.Database.Migrate();
                TenantContextSeed.SeedAsync(context, logger, serviceProvider).Wait();
            });
            
            logger.LogInformation("Migrated database associated");
        }
        catch (Exception e)
        {
            logger.LogError(e, "An error occurred while migrating the database");
        }
        return app;
    }
}