namespace ManeKani.Auth.ApiKey;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ApiKeyAuthorizationFilter : IAsyncAuthorizationFilter
{
    private const string ApiKeyHeader = "X-API-KEY";

    private readonly ILogger<ApiKeyAuthorizationFilter> _logger;
    private readonly IApiKeyValidator _apiKeyValidator;
    private readonly ApiKeyScopes.Scope[] _scopes;

    public ApiKeyAuthorizationFilter(ApiKeyScopes.Scope[] scopes, ILogger<ApiKeyAuthorizationFilter> logger, IApiKeyValidator apiKeyValidator)
    {
        _apiKeyValidator = apiKeyValidator;
        _logger = logger;
        _scopes = scopes;

    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        _logger.LogInformation("API Key Authorization Filter");
        var key = context.HttpContext.Request.Headers[ApiKeyHeader];


        if (string.IsNullOrEmpty(key))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var validationResult = await _apiKeyValidator.IsValid(key!, _scopes);
        _logger.LogInformation("API Key Validation Result: {}", validationResult);
        if (!validationResult)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        context.HttpContext.Items["ApiKey"] = validationResult.ApiKey!;
        context.HttpContext.Items["User"] = validationResult.User!;

        _logger.LogInformation("API key authorized: {}", validationResult.ApiKey!.Id);
    }
}