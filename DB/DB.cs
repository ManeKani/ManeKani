namespace ManeKani.DB;

using Dapper;
using ManeKani.Core.Interfaces;
using ManeKani.Core.Models;
using Npgsql;
using SqlKata.Compilers;
using SqlKata.Execution;


public class ManeKaniDatabase : Repository, IApiKeyRepository, IUserRepository, IDeckRepository
{
    private readonly ApiKeysRepository _apiKeyRepository;
    private readonly UserRepository _userRepository;
    private readonly DeckRepository _deckRepository;

    public ManeKaniDatabase(string? connectionString)
    {
        var connection = new NpgsqlConnection(connectionString);
        Database = new QueryFactory(connection, new PostgresCompiler());

        _apiKeyRepository = new ApiKeysRepository { Database = Database };
        _userRepository = new UserRepository { Database = Database };
        _deckRepository = new DeckRepository { Database = Database };

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

    public Task<ApiKey?> GetApiKeyByHash(string apiKeyHash)
    {
        return ((IApiKeyRepository)_apiKeyRepository).GetApiKeyByHash(apiKeyHash);
    }

    public Task<bool> IsUserComplete(Guid userId)
    {
        return ((IUserRepository)_userRepository).IsUserComplete(userId);
    }

    public Task<User> GetUser(Guid userId)
    {
        return ((IUserRepository)_userRepository).GetUser(userId);
    }

    public Task<Deck> CreateDeck(Guid userId, Deck request)
    {
        return ((IDeckRepository)_deckRepository).CreateDeck(userId, request);
    }

    public Task<Deck?> GetDeck(Guid deckId)
    {
        return ((IDeckRepository)_deckRepository).GetDeck(deckId);
    }

    public Task<IEnumerable<DeckBasic>> GetDecks(Guid userId)
    {
        return ((IDeckRepository)_deckRepository).GetDecks(userId);
    }

    public Task<Deck> UpdateDeck(Guid deckId, Deck request)
    {
        return ((IDeckRepository)_deckRepository).UpdateDeck(deckId, request);
    }

    public Task DeleteDeck(Guid deckId)
    {
        return ((IDeckRepository)_deckRepository).DeleteDeck(deckId);
    }

    public Task<ApiKey> UseApiKey(Guid apiKeyId)
    {
        return ((IApiKeyRepository)_apiKeyRepository).UseApiKey(apiKeyId);
    }
}
