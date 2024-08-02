using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ManeKani.Auth.Ory;

public class OryAuthenticationHandler : SignInAuthenticationHandler<OryAuthenticationOptions>
{
    private Task<AuthenticateResult>? _fetchCookieTask;


    public OryAuthenticationHandler(IOptionsMonitor<OryAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder)
    : base(options, logger, encoder)
    {
    }

    private Task<AuthenticateResult> EnsureTicket()
    {
        if (_fetchCookieTask is null)
        {
            _fetchCookieTask = FetchCookieSession();
        }
        return _fetchCookieTask;
    }

    private async Task<AuthenticateResult> FetchCookieSession()
    {
        var cookie = CookieManager.GetRequestCookie(Context, Options.SessionKey!);
        if (string.IsNullOrEmpty(cookie))
        {
            Logger.LogInformation("No session cookie found");
            return AuthenticateResults.MissingSessionCookie;
        }

        var session = await Options.Ory.ToSessionAsync(cookie: $"{Options.SessionKey}={cookie}");
        if (session is null)
        {
            Logger.LogInformation("Failed to retrieve session");
            return AuthenticateResults.FailedToRetrieveSession;
        }
        if (!session.Active)
        {
            Logger.LogInformation("Session is not active");
            return AuthenticateResults.ExpiredTicket;
        }

        var ticket = OryIdentity.AuthenticationTicket(Scheme.Name, session);

        return AuthenticateResult.Success(ticket);
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var result = await EnsureTicket();
        if (!result.Succeeded)
        {
            Logger.LogInformation("Failed to retrieve session: {}", result.Failure);
            return result;
        }

        var ticket = result!;
        if (ticket.Principal is null)
        {
            return AuthenticateResults.NoPrincipal;
        }

        return AuthenticateResult.Success(new AuthenticationTicket(ticket.Principal, ticket.Properties, Scheme.Name));
    }

    protected override Task HandleSignInAsync(ClaimsPrincipal user, AuthenticationProperties? properties)
    {
        throw new NotImplementedException();
    }

    protected override Task HandleSignOutAsync(AuthenticationProperties? properties)
    {
        throw new NotImplementedException();
    }
}

public static class CookieManager
{
    public static string? GetRequestCookie(HttpContext ctx, string key)
    {
        ctx.Request.Cookies.TryGetValue(key, out string? value);
        return value;
    }

    public static void DeleteCookie(HttpContext ctx, string key)
    {
        ctx.Response.Cookies.Delete(key);
    }
}

