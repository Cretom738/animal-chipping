using Microsoft.AspNetCore.Authentication;

namespace WebApi.Authentication.Extensions
{
    public static class BasicAuthenticationExtension
    {
        public static void AddBasic(this AuthenticationBuilder builder, 
            Action<AuthenticationSchemeOptions>? options = null)
        {
            builder.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", options);
        }
    }
}
