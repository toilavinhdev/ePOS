using ePOS.Shared.ValueObjects;
using Microsoft.Extensions.FileProviders;

namespace ePOS.API.Configurations;

public static class StaticFileSetup
{
    public static IApplicationBuilder UseStaticFileSetup(this IApplicationBuilder app, AppSettings appSettings)
    {
        var storageConfig = appSettings.StorageConfig;
        if (!Directory.Exists(storageConfig.Location)) Directory.CreateDirectory(storageConfig.Location);
        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(storageConfig.Location),
            RequestPath = new PathString(storageConfig.ExternalPath)
        });
        return app;
    }
}