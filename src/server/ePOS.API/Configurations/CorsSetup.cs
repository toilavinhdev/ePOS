namespace ePOS.API.Configurations;

public static class CorsSetup
{
    public static IApplicationBuilder UseCorsSetup(this IApplicationBuilder app)
    {
        app.UseCors(policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        });
        return app;
    }
}