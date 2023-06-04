using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Principal;

namespace WebApi.Filters
{
    public class InvalidAuthenticationFilterAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            HttpContext httpContext = context.HttpContext;
            IHeaderDictionary headers = httpContext.Request.Headers;
            IIdentity? identity = httpContext.User.Identity;
            bool hasAuthorizationHeader = headers.ContainsKey("Authorization");
            bool isAuthenticated = identity?.IsAuthenticated ?? false;
            if (hasAuthorizationHeader && !isAuthenticated)
            {
                context.Result = new UnauthorizedResult();
            }
            return Task.CompletedTask;
        }
    }
}
