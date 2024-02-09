using ePOS.Application;
using ePOS.Application.Common.Mediator;
using ePOS.Shared.Exceptions;
using FluentValidation;
using MediatR;

namespace ePOS.API.Configurations;

public static class ValidationSetup
{
    public static IServiceCollection AddValidationSetup(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<IAssemblyMaker>();
        
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidatorBehavior<,>));
        
        services.AddControllers().ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = action =>
            {
                var errors = action.ModelState.Values.Where(x => x.Errors.Count > 0)
                    .SelectMany(y => y.Errors)
                    .Select(x => x.ErrorMessage)
                    .ToArray();
                throw new BadRequestException(string.Join(";", errors));
            };
        });
        
        return services;
    }
}