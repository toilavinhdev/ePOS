using Serilog;

namespace ePOS.API.Configurations;

public static class SerilogSetup
{
    public static void Setup(WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .CreateLogger();
        builder.Host.UseSerilog();
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();
    }
}