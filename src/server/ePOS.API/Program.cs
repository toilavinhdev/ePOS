using ePOS.API.Configurations;
using ePOS.API.Middlewares;
using ePOS.Application;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;
EnvironmentSetup.Setup(environment, out var appSettings);
SerilogSetup.Setup(builder);

var services = builder.Services;
services.AddSingleton(appSettings);
services.AddSwaggerSetup();
services.AddHttpContextAccessor();
services.AddPersistenceSetup(appSettings);
services.AddIdentitySetup(appSettings);
services.AddValidationSetup();
services.AddMediatR(config => config.RegisterServicesFromAssembly(ApplicationAssembly.Assembly));
services.AddAutoMapper(ApplicationAssembly.Assembly);
services.AddServiceRegistrations();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseCorsSetup();
app.UseStaticFileSetup(appSettings);
app.UseSwaggerSetup();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MigrateDatabase();

app.Run();