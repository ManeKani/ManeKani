using ManeKani.Auth.Ory;
using ManeKani.DB;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ManeKani.Auth.Policies;

public static class LoginPolicy
{
    public const string DenyIncomplete = "deny_incomplete";
    public const string AllowIncomplete = "allow_incomplete";

    public static void AddLoginPolicy(this AuthorizationOptions options)
        => options.AddLoginPolicy(OryAuthenticationDefaults.AuthenticationScheme);

    public static void AddLoginPolicy(this AuthorizationOptions options, string scheme)
    {
        options.AddPolicy(DenyIncomplete, policy =>
            policy
                .BaseLoginPolicy(scheme)
                .Requirements.Add(new CompletedUserRequirement())
        );
        options.AddPolicy(AllowIncomplete, policy => policy.BaseLoginPolicy(scheme));

    }

    private static AuthorizationPolicyBuilder BaseLoginPolicy(this AuthorizationPolicyBuilder policy, string scheme)
        => policy.RequireAuthenticatedUser()
            .AddAuthenticationSchemes(scheme);
}

public class CompletedUserRequirement : IAuthorizationRequirement
{ }

public class CompletedUserRequirementHandler(ManeKaniDatabase db) : AuthorizationHandler<CompletedUserRequirement>
{
    readonly ManeKaniDatabase _db = db;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, CompletedUserRequirement requirement)
    {
        var user = new OryUser(context.User);
        if (await _db.IsUserComplete(user.Id))
        {
            context.Succeed(requirement);
        }
        else
        {
            if (context.Resource is HttpContext httpContext)
            {
                httpContext.Response.Redirect("/settings/account");
            }
            context.Succeed(requirement);
        }
    }
}