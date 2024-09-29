namespace ManeKani.Core.Interfaces;

using ManeKani.Core.Models;

public interface IUserRepository
{
    public Task<bool> IsUserComplete(Guid userId);
    public Task<User> GetUser(Guid userId);
}