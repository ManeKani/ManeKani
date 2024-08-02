using System.Security.Claims;
using System.Text.Encodings.Web;
using ManeKani.DB;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ManeKani.Auth.ApiKey;

public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions>
{

    public ApiKeyAuthenticationHandler(IOptionsMonitor<ApiKeyAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ManeKaniDatabase db)
    : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        throw new NotImplementedException();
    }
}