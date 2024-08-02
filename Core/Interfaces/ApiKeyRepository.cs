using ManeKani.Core.Models;

namespace ManeKani.Core.Interfaces;

public interface IApiKeyRepository
{
    public Task<uint> GetUserApiKeyCount(Guid userId);
    public Task<ApiKey> CreateUserApiKey(Guid userId, ApiKey request);
    public Task<PublicApiKey?> GetUserApiKey(Guid userId, Guid apiKeyId);
    public Task<IEnumerable<PublicApiKey>> GetUserApiKeys(Guid userId);
    public Task DeleteUserApiKey(Guid userId, Guid apiKeyId);
}