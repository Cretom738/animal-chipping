using Core.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Infrastructure.Data.Extensions;

namespace WebApi.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        readonly ILogger<BasicAuthenticationHandler> _logger;
        readonly ChippedAnimalsDbContext _context;

        public BasicAuthenticationHandler(
            ChippedAnimalsDbContext context, 
            IOptionsMonitor<AuthenticationSchemeOptions> options, 
            ILoggerFactory logger, 
            UrlEncoder encoder, 
            ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {
            _logger = logger.CreateLogger<BasicAuthenticationHandler>();
            _context = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string? authenticationHeaderValue = Request.Headers.Authorization;
            _logger.LogDebug("Authentication header value {headerValue}", authenticationHeaderValue);
            if (authenticationHeaderValue == null)
            {
                return AuthenticateResult.Fail("Authorization header is absent");
            }
            if (!IsCorrectScheme(authenticationHeaderValue))
            {
                return AuthenticateResult.Fail($"Authorization scheme is not {Scheme.Name}");
            }
            if (!TryGetCredentialsFromBase64(authenticationHeaderValue,
                out string? email, out string? password))
            {
                return AuthenticateResult.Fail("Authorization header Value is invalid");
            }
            Account? account = await _context.Accounts.FetchByEmailNoTrackingAsync(email!);
            if (!AreCorrectCredentials(account, email!, password!))
            {
                return AuthenticateResult.Fail($"Wrong credentials");
            }
            _logger.LogInformation("User with email {email} has been authenticated", email);
            return AuthenticateResult.Success(CreateTicket(email!, account!.Role.Role));
        }

        bool IsCorrectScheme(string authenticationHeaderValue)
        {
            return authenticationHeaderValue.StartsWith(
                            Scheme.Name, StringComparison.OrdinalIgnoreCase);
        }

        bool TryGetCredentialsFromBase64(string base64, out string? email, out string? password)
        {
            email = null;
            password = null;
            string encodedCredentials = base64.Substring(Scheme.Name.Length + 1);
            byte[] bytes = new byte[encodedCredentials.Length * 3 / 4];
            if (!Convert.TryFromBase64String(encodedCredentials, bytes, out _))
            {
                return false;
            }
            string decodedCredentials = Encoding.UTF8.GetString(bytes).TrimEnd('\u0000');
            _logger.LogDebug("Authentication credentials: {credentials}", decodedCredentials);
            string[] credentials = decodedCredentials.Split(":");
            if (credentials.Length != 2)
            {
                return false;
            }
            email = credentials[0];
            password = credentials[1];
            return true;
        }

        bool AreCorrectCredentials(Account? account, string email, string password)
        {
            return account != null
                && account.Email.ToLower() == email.ToLower()
                && account.Password == password;
        }

        AuthenticationTicket CreateTicket(string email, string role)
        {
            Claim[] claims = new Claim[] {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Role, role)
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, Scheme.Name);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);
            return new AuthenticationTicket(principal, Scheme.Name);
        }
    }
}
