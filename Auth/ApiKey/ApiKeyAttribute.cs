using Microsoft.AspNetCore.Mvc;

namespace ManeKani.Auth.ApiKey;

public class ApiKeyAttribute : TypeFilterAttribute
{




    public ApiKeyAttribute() : this([])
    {
    }

    public ApiKeyAttribute(ApiKeyScopes.Scope scope) : this([scope])
    {
    }

    public ApiKeyAttribute(ApiKeyScopes.Scope[] requiredScopes) : base(typeof(ApiKeyAuthorizationFilter))
    {
        Arguments = [requiredScopes];
    }
}