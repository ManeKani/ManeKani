namespace ManeKani.Core.Adapters;

using System.Security.Cryptography;
using ManeKani.Core.Interfaces;
using ManeKani.Core.Models;
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
}

class ApiKeySecrets
{
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
        var randomBytes = new byte[4];
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

    public string Hash { get; }
    public string Prefix { get; }
    private string Token { get; }
    public string Key => $"{Prefix}.{Token}";
}
