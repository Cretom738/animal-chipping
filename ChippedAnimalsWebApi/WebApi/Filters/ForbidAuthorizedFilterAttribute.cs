using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Principal;

namespace WebApi.Filters
{
    public class ForbidAuthenticatedFilterAttribute : Attribute, IAsyncAuthorizationFilter
    {
        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            IIdentity? identity = context.HttpContext.User.Identity;
            bool isAuthenticated = identity?.IsAuthenticated ?? false;
            if (isAuthenticated)
            {
                context.Result = new ForbidResult("Basic");
            }
            return Task.CompletedTask;
        }
    }
}
