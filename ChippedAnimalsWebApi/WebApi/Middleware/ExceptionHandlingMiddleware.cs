using Core.Exceptions;
using Services.Dtos;
using System.Text.Json;

namespace WebApi.Middleware
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                await HandleException(context, e);
            }
        }

        async Task HandleException(HttpContext context, Exception exception)
        {
            int statusCode = exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                UnauthorizedException => StatusCodes.Status401Unauthorized,
                ForbiddenException => StatusCodes.Status403Forbidden,
                NotFoundException => StatusCodes.Status404NotFound,
                ConflictException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };
            context.Response.StatusCode = statusCode;
            if (statusCode == StatusCodes.Status500InternalServerError)
            {
                _logger.LogError(exception, exception.Message);
                return;
            }
            _logger.LogWarning(exception, exception.Message);
            context.Response.ContentType = "application/json";
            var responseBody = new ErrorDto
            {
                Error = exception.Message
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(responseBody));
        } 
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }

        public static void AddExceptionHandlingMiddleware(this IServiceCollection services)
        {
            services.AddScoped<ExceptionHandlingMiddleware>();
        }
    }
}
