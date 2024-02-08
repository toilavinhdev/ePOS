using ePOS.Shared.ValueObjects;

namespace ePOS.API.Configurations;

public static class EnvironmentSetup
{
    public static void Setup(IWebHostEnvironment environment, out AppSettings appSettings)
    {
        appSettings = new AppSettings();
        var path = Path.Combine(nameof(AppSettings), $"appsettings.{environment.EnvironmentName}.json");
        new ConfigurationBuilder()
            .SetBasePath(environment.ContentRootPath)
            .AddJsonFile(path)
            .AddEnvironmentVariables()
            .Build()
            .Bind(appSettings);
    }
}