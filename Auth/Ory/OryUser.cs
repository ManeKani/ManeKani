using System.Security.Claims;

namespace ManeKani.Auth.Ory;

public class OryUser(ClaimsPrincipal principal) : ClaimsPrincipal(principal)
{
    public Guid Id { get => Guid.Parse(base.Identity!.Name!); }
    public string Email { get => base.Claims.First(c => c.Type == ClaimTypes.Email).Value; }
}