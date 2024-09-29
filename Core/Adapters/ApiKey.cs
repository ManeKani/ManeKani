namespace ManeKani.Core.Adapters;

using System.Security.Cryptography;
using ManeKani.Core.Interfaces;
using ManeKani.Core.Models;
using Newtonsoft.Json;
using SimpleBase;



public struct CreateApiKeyRequest
{
    public string Name;
    public ApiKeyClaims Claims;
}

public class CreateApiKeyResponse : PublicApiKey
{
    public required string Key { get; set; }
}

public static class ApiKeysAdapter
{
    public static async Task<CreateApiKeyResponse> CreateApiKey(IApiKeyRepository repo, Guid userId, CreateApiKeyRequest request)
    {
        if (await repo.GetUserApiKeyCount(userId) >= 20)
        {
            throw new Exception("You have reached the maximum number of API keys");
        }

        var keySecrets = new ApiKeySecrets();

        var apiKey = new ApiKey
        {
            Hash = keySecrets.Hash,
            Prefix = keySecrets.Prefix,
            Name = request.Name,
            Claims = request.Claims,
        };

        var createKey = await repo.CreateUserApiKey(userId, apiKey);

        return new CreateApiKeyResponse
        {
            Id = createKey.Id,
            Name = createKey.Name,
            Prefix = createKey.Prefix,
            Claims = createKey.Claims,
            CreatedAt = createKey.CreatedAt,
            UpdatedAt = createKey.UpdatedAt,
            CreatedByUserId = createKey.CreatedByUserId,
            Key = keySecrets.Key,
        };
    }

    public class ApiKeyValidationResult
    {
        public bool IsValid { get; set; }
        public ApiKey? ApiKey { get; set; }

        public static ApiKeyValidationResult Invalid => new() { IsValid = false, ApiKey = null };
        public static ApiKeyValidationResult Valid(ApiKey apiKey) => new() { IsValid = true, ApiKey = apiKey };


        public static implicit operator bool(ApiKeyValidationResult result) => result.IsValid;
    }
    public static async Task<ApiKeyValidationResult> IsApiKeyValid(IApiKeyRepository repo, string key, ApiKeyClaims claims)
    {
        var isKeyValid = ApiKeySecrets.TryParseKey(key, out var hash);
        if (!isKeyValid)
        {
            return ApiKeyValidationResult.Invalid;
        }

        var apiKey = await repo.GetApiKeyByHash(hash);

        // the key does not exist or has been revoked
        if (apiKey == null || apiKey.RevokedAt.HasValue)
        {
            return ApiKeyValidationResult.Invalid;
        }


        // check if api key claims has all the required claims
        if (claims.DeckWrite && !apiKey.Claims.DeckWrite
            || claims.DeckDelete && !apiKey.Claims.DeckDelete
            || claims.SubjectWrite && !apiKey.Claims.SubjectWrite
            || claims.SubjectDelete && !apiKey.Claims.SubjectDelete
            || claims.ReviewCreate && !apiKey.Claims.ReviewCreate
            || claims.StudyDataWrite && !apiKey.Claims.StudyDataWrite
            || claims.StudyDataDelete && !apiKey.Claims.StudyDataDelete
        )
        {
            return ApiKeyValidationResult.Invalid;
        }


        // updated used at date
        var updatedKey = await repo.UseApiKey(apiKey.Id);

        return ApiKeyValidationResult.Valid(updatedKey);
    }
}

class ApiKeySecrets
{
    private const string KeySeparator = ".";
    private const int PrefixLength = 4;

    public string Hash { get; }
    public string Prefix { get; }
    private string Token { get; }
    public string Key => $"{Prefix}{KeySeparator}{Token}";

    public ApiKeySecrets()
    {
        // generate a random prefix
        var prefixBytes = GetRandomPrefixBytes();
        Prefix = Convert.ToHexString(prefixBytes);

        // generate a random token
        var tokenBytes = Guid.NewGuid().ToByteArray();
        Token = Base58.Bitcoin.Encode(tokenBytes);

        // generate a hash of the token
        var hashBytes = HashToken(prefixBytes, tokenBytes);
        Hash = Convert.ToHexString(hashBytes);
    }

    private static byte[] GetRandomPrefixBytes()
    {
        var randomBytes = new byte[PrefixLength];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        return randomBytes;
    }

    private static byte[] HashToken(byte[] prefixBytes, byte[] tokenBytes)
    {
        var hasher = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        hasher.AppendData(prefixBytes);
        hasher.AppendData(tokenBytes);
        return hasher.GetHashAndReset();
    }

    public static bool TryParseKey(string key, out string hash)
    {
        var parts = key.Split(KeySeparator);
        hash = string.Empty;

        if (parts.Length != 2)
        {
            return false;
        }

        var prefix = parts[0];
        var token = parts[1];

        if (prefix.Length != PrefixLength * 2 || token.Length == 0)
        {
            return false;
        }

        var prefixBytes = Convert.FromHexString(prefix);
        var tokenBytes = Base58.Bitcoin.Decode(token);

        hash = Convert.ToHexString(HashToken(prefixBytes, tokenBytes));
        return true;
    }
}
