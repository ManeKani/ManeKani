using Dapper;
using ManeKani.Core.Interfaces;
using ManeKani.Core.Models;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace ManeKani.DB;

public class ManeKaniDatabase : Repository, IApiKeyRepository, IUserRepository
{
    private readonly ApiKeysRepository _apiKeyRepository;
    private readonly UserRepository _userRepository;

    public ManeKaniDatabase(string? connectionString)
    {
        var connection = new NpgsqlConnection(connectionString);
        Database = new QueryFactory(connection, new PostgresCompiler());

        _apiKeyRepository = new ApiKeysRepository { Database = Database };
        _userRepository = new UserRepository { Database = Database };

        SqlMapper.AddTypeHandler(new ApiKeyClaimsHandler());
    }

    public Task<ApiKey> CreateUserApiKey(Guid userId, ApiKey request)
    {
        return ((IApiKeyRepository)_apiKeyRepository).CreateUserApiKey(userId, request);
    }

    public Task DeleteUserApiKey(Guid userId, Guid apiKeyId)
    {
        return ((IApiKeyRepository)_apiKeyRepository).DeleteUserApiKey(userId, apiKeyId);
    }

    public Task<PublicApiKey?> GetUserApiKey(Guid userId, Guid apiKeyId)
    {
        return ((IApiKeyRepository)_apiKeyRepository).GetUserApiKey(userId, apiKeyId);
    }

    public Task<uint> GetUserApiKeyCount(Guid userId)
    {
        return ((IApiKeyRepository)_apiKeyRepository).GetUserApiKeyCount(userId);
    }

    public Task<IEnumerable<PublicApiKey>> GetUserApiKeys(Guid userId)
    {
        return ((IApiKeyRepository)_apiKeyRepository).GetUserApiKeys(userId);
    }

    public Task<bool> IsUserComplete(Guid userId)
    {
        return ((IUserRepository)_userRepository).IsUserComplete(userId);
    }
}
