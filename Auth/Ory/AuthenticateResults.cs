using Microsoft.AspNetCore.Authentication;

namespace ManeKani.Auth.Ory;

internal static class AuthenticateResults
{
    internal static AuthenticateResult FailedToRetrieveSession = AuthenticateResult.Fail("Invalid authentication session");
    internal static AuthenticateResult MissingSessionCookie = AuthenticateResult.Fail("No session cookie found");
    internal static AuthenticateResult MissingSessionId = AuthenticateResult.Fail("SessionId missing");
    internal static AuthenticateResult MissingIdentityInSession = AuthenticateResult.Fail("Identity missing in session store");
    internal static AuthenticateResult ExpiredTicket = AuthenticateResult.Fail("Ticket expired");
    internal static AuthenticateResult NoPrincipal = AuthenticateResult.Fail("No principal.");

}