using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters
{
    public class RootPathFilterAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            HttpContext httpContext = context.HttpContext;
            string scheme = httpContext.Request.Scheme;
            string host = httpContext.Request.Host.Value;
            httpContext.Items["rootPath"] = $"{scheme}://{host}";
            await next();
        }
    }
}
