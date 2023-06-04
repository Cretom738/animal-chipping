namespace WebApi.Middleware
{
    public class HttpContextLoggingMiddleware : IMiddleware
    {
        readonly ILogger<HttpContextLoggingMiddleware> _logger;

        public HttpContextLoggingMiddleware(ILogger<HttpContextLoggingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            _logger.LogInformation("RemoteIpAddress: {ip}", 
                httpContext.Connection.RemoteIpAddress);
            await next(httpContext);
        }
    }

    public static class HttpContextLoggingExtensions
    {
        public static IApplicationBuilder UseHttpContextLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HttpContextLoggingMiddleware>();
        }

        public static void AddHttpContextLoggingMiddleware(this IServiceCollection services)
        {
            services.AddScoped<HttpContextLoggingMiddleware>();
        }
    }
}
