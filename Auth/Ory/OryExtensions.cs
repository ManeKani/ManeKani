using Microsoft.AspNetCore.Authentication;

namespace ManeKani.Auth.Ory;

public static class OryExtensions
{
    public static AuthenticationBuilder AddOry(this AuthenticationBuilder builder, Action<OryAuthenticationOptions> options)
        => builder.AddOry(OryAuthenticationDefaults.AuthenticationScheme, displayName: null, options: options);

    public static AuthenticationBuilder AddOry(this AuthenticationBuilder builder, string scheme, Action<OryAuthenticationOptions> options)
        => builder.AddOry(scheme, displayName: null, options: options);

    public static AuthenticationBuilder AddOry(this AuthenticationBuilder builder, string scheme, string? displayName, Action<OryAuthenticationOptions> options)
    {
        builder.Services.AddOptions<OryAuthenticationOptions>(scheme);
        return builder.AddScheme<OryAuthenticationOptions, OryAuthenticationHandler>(scheme, displayName, options);
    }
}