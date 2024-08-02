using Microsoft.AspNetCore.Authentication;

namespace ManeKani.Auth.ApiKey;

public static class ApiKeyExtensions
{
    public static AuthenticationBuilder AddApiKey(this AuthenticationBuilder builder, string scheme, string? displayName, Action<ApiKeyAuthenticationOptions> options)
    {
        builder.Services.AddOptions<ApiKeyAuthenticationOptions>(scheme);
        throw new NotImplementedException();
        // return builder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(scheme, displayName, options);
    }
}