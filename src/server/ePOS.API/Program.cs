using ePOS.API.Configurations;
using ePOS.API.Middlewares;
using ePOS.Application;
using ePOS.Application.Common.Mediator;
using FluentValidation;
using MediatR;

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
services.AddValidatorsFromAssemblyContaining<IAssemblyMaker>();
services.AddMediatR(config => config.RegisterServicesFromAssemblyContaining<IAssemblyMaker>());
services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
services.AddAutoMapper(typeof(IAssemblyMaker));
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