using ManeKani.Auth.Ory;
using ManeKani.DB;
using Microsoft.AspNetCore.Authorization;

namespace ManeKani.Auth.Policies;

public static class LoginPolicy
{
    public const string LoginComplete = "login_complete";
    public const string LoginIncomplete = "login_incomplete";

    public static void AddLoginPolicy(this AuthorizationOptions options)
        => options.AddLoginPolicy(OryAuthenticationDefaults.AuthenticationScheme);

    public static void AddLoginPolicy(this AuthorizationOptions options, string scheme)
    {
        options.AddPolicy(LoginIncomplete, policy => policy.BaseLoginPolicy(scheme));

        options.AddPolicy(LoginComplete, policy => policy
            .BaseLoginPolicy(scheme)
            .Requirements.Add(new CompletedUserRequirement())
        );
    }

    private static AuthorizationPolicyBuilder BaseLoginPolicy(this AuthorizationPolicyBuilder policy, string scheme)
        => policy.RequireAuthenticatedUser()
            .AddAuthenticationSchemes(scheme);
}

public class CompletedUserRequirement : IAuthorizationRequirement
{ }

public class CompletedUserHandler(ManeKaniDatabase db) : AuthorizationHandler<CompletedUserRequirement>
{
    readonly ManeKaniDatabase _db = db;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CompletedUserRequirement requirement)
    {
        var user = new OryUser(context.User);
        if (await _db.IsUserComplete(user.Id))
        {
            context.Succeed(requirement);
        }
    }
}