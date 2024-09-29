using ManeKani.Core.Adapters;
using ManeKani.Core.Interfaces;
using ManeKani.Core.Models;
using ManeKani.DB;

namespace ManeKani.Auth.ApiKey;

public class ApiKeyValidator : IApiKeyValidator
{
    private readonly ManeKaniDatabase _db;

    public ApiKeyValidator(ManeKaniDatabase db)
    {
        _db = db;
    }

    public async Task<UserApiKeyValidationResult> IsValid(string key, ApiKeyScopes.Scope[] scopes)
    {
        var validationResult = await ApiKeysAdapter.IsApiKeyValid(_db, key, ApiKeyScopes.IntoClaims(scopes));
        if (validationResult)
        {
            var user = await UsersAdapter.GetUser(_db, validationResult.ApiKey!.CreatedByUserId);
            if (user == null)
            {
                return UserApiKeyValidationResult.Invalid;
            }

            return UserApiKeyValidationResult.Valid(validationResult.ApiKey!, user!);
        }
        return UserApiKeyValidationResult.Invalid;
    }
}

public interface IApiKeyValidator
{
    Task<UserApiKeyValidationResult> IsValid(string apiKey, ApiKeyScopes.Scope[] scopes);
}

public class UserApiKeyValidationResult
{
    public bool IsValid { get; set; }
    public Core.Models.ApiKey? ApiKey { get; set; }
    public User? User { get; set; }

    public static UserApiKeyValidationResult Invalid => new() { IsValid = false, ApiKey = null, User = null };
    public static UserApiKeyValidationResult Valid(Core.Models.ApiKey apiKey, User user) => new() { IsValid = true, ApiKey = apiKey, User = user };

    public static implicit operator bool(UserApiKeyValidationResult r)
    {
        return r.IsValid;
    }
}