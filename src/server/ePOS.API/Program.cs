using ePOS.API.Configurations;
using ePOS.API.Middlewares;
using ePOS.Application;
using FluentValidation;

var builder = WebApplication.CreateBuilder(args);
var environment = builder.Environment;
EnvironmentSetup.Setup(environment, out var appSettings);
SerilogSetup.Setup(builder);

var services = builder.Services;
services.AddSingleton(appSettings);
services.AddSwaggerSetup();
services.AddHttpContextAccessor();
services.AddValidatorsFromAssemblyContaining<IAssemblyMaker>();
services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<IAssemblyMaker>());
services.AddAutoMapper(typeof(IAssemblyMaker));
services.AddServiceRegistrations();

var app = builder.Build();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseCorsSetup();
app.UseSwaggerSetup();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();