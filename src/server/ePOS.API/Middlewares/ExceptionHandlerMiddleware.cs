using System.Net;
using System.Text.Json;
using ePOS.Shared.Exceptions;
using ePOS.Shared.ValueObjects;
using FluentValidation;

namespace ePOS.API.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError("An exception occurred: {0}", ex);
            switch (ex)
            {
                case UnauthorizedAccessException:
                    await WriteResponseAsync(context, (int)HttpStatusCode.Unauthorized, "Unauthorized");
                    break;
                case BadRequestException:
                    await WriteResponseAsync(context, (int)HttpStatusCode.BadRequest, GetErrorMessage(ex));
                    break;
                case ValidationException:
                    await WriteResponseAsync(context, (int)HttpStatusCode.BadRequest, GetErrorMessage(ex));
                    break;
                case CustomException customException:
                    await WriteResponseAsync(context, customException.StatusCode, GetErrorMessage(ex));
                    break;
                default:
                    await WriteResponseAsync(context, (int)HttpStatusCode.InternalServerError, GetErrorMessage(ex));
                    break;
            }
        }
    }
    
    private static async Task WriteResponseAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(new APIResponse()
        {
            Success = false,
            StatusCode = statusCode,
            Message = message,
        }));
    }
    
    private static string GetErrorMessage(Exception ex)
    {
        if (ex is ValidationException fEx)
        {
            return string.Join(";",fEx.Errors.Select(t => t.ErrorMessage).Distinct().ToList()) ;
        }
        return string.Join(";", _().Distinct().ToList());
        IEnumerable<string> _()
        {
            yield return ex.Message;
            var innerEx = ex.InnerException;
            var count = 5;
            while (innerEx != null && count-- > 0)
            {
                yield return innerEx.Message;
                innerEx = innerEx.InnerException;
            }
        }
    }
}