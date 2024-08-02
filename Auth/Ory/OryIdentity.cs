using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Ory.Client.Model;



public class OryIdentity : IIdentity
{
    public OryIdentity(string scheme, ClientSession session)
    {
        // id.Traits.
        AuthenticationType = scheme;

        IsAuthenticated = session.Active;

        var traitsObj = (JObject)session.Identity.Traits;
        var metadataObj = (JObject)session.Identity.MetadataPublic;

        Email = traitsObj["email"]?.ToString() ?? "";
        Id = metadataObj["id"]?.ToString() ?? "";
    }

    public static AuthenticationTicket AuthenticationTicket(string scheme, ClientSession session)
    {
        var oryIdentity = new OryIdentity(scheme, session);

        var claimsIdentity = new ClaimsIdentity(identity: oryIdentity, claims: [
            new Claim(ClaimTypes.Email, oryIdentity.Email),
        ]);
        var ticket = new AuthenticationTicket(
            new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties(), scheme
        );

        return ticket;
    }


    public string Id { get; }
    public string Email { get; }

    public string? AuthenticationType { get; }
    public bool IsAuthenticated { get; }
    public string Name { get => Id; }


}