using System.Data;

using System.Text.Json;
using Dapper;
using ManeKani.Core.Interfaces;
using ManeKani.Core.Models;
using SqlKata.Execution;

namespace ManeKani.DB;

public class ApiKeysRepository : Repository, IApiKeyRepository
{
    public Task<ApiKey> CreateUserApiKey(Guid userId, ApiKey request)
    {
        return Database.Connection.QuerySingleAsync<ApiKey>(@"
            INSERT INTO api_keys (claims, name, prefix, hash, created_by_user_id)
                VALUES (@claims::JSONB, @name, @prefix, @hash, @created_by_user_id)
            RETURNING id, hash, name, prefix, claims, used_at, revoked_at, created_at, updated_at, created_by_user_id", new
        {
            claims = JsonSerializer.Serialize(request.Claims),
            name = request.Name,
            prefix = request.Prefix,
            hash = request.Hash,
            created_by_user_id = userId,
        });
    }

    public Task DeleteUserApiKey(Guid userId, Guid apiKeyId)
    {
        return Database.Query("api_keys")
            .Where("created_by_user_id", userId)
            .Where("id", apiKeyId)
            .DeleteAsync();
    }

    public Task<PublicApiKey?> GetUserApiKey(Guid userId, Guid apiKeyId)
    {
        return Database.Query("api_keys")
            .Where("created_by_user_id", userId)
            .Where("id", apiKeyId)
            .Select("api_keys.{id, name, prefix, claims, used_at, revoked_at, created_at, updated_at, created_by_user_id}")
            .FirstOrDefaultAsync<PublicApiKey?>();
    }

    public Task<uint> GetUserApiKeyCount(Guid userId)
    {
        return Database.Query("api_keys")
            .Where("created_by_user_id", userId)
            .CountAsync<uint>();
    }

    public Task<IEnumerable<PublicApiKey>> GetUserApiKeys(Guid userId)
    {
        return Database.Query("api_keys")
            .Where("created_by_user_id", userId)
            .Select("api_keys.{id, name, prefix, claims, used_at, revoked_at, created_at, updated_at, created_by_user_id}")
            .OrderByDesc("created_at")
            .GetAsync<PublicApiKey>();
    }

    public Task<ApiKey?> GetApiKeyByHash(string apiKeyHash)
    {
        return Database.Query("api_keys")
            .Where("hash", apiKeyHash)
            .Select("api_keys.{id, name, prefix, claims, used_at, revoked_at, created_at, updated_at, created_by_user_id}")
            .FirstOrDefaultAsync<ApiKey?>();
    }

    public Task<ApiKey> UseApiKey(Guid apiKeyId)
    {
        return Database.Connection.QuerySingleAsync<ApiKey>(@"
            UPDATE api_keys
                SET used_at = NOW()
            WHERE id = @id
            RETURNING id, hash, name, prefix, claims, used_at, revoked_at, created_at, updated_at, created_by_user_id", new
        {
            id = apiKeyId,
        });
    }
}


public class ApiKeyClaimsHandler : SqlMapper.TypeHandler<ApiKeyClaims>
{
    public override ApiKeyClaims Parse(object value)
    {
        var json = (string)value;
        return JsonSerializer.Deserialize<ApiKeyClaims>(json);
    }

    public override void SetValue(IDbDataParameter parameter, ApiKeyClaims value)
    {
        parameter.Value = JsonSerializer.Serialize(value);
    }
}