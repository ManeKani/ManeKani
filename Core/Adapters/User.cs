namespace ManeKani.Core.Adapters;

using ManeKani.Core.Interfaces;
using ManeKani.Core.Models;

public static class UsersAdapter
{
    public static async Task<bool> IsUserComplete(IUserRepository repo, Guid userId)
    {
        return await repo.IsUserComplete(userId);
    }

    public static async Task<User> GetUser(IUserRepository repo, Guid userId)
    {
        return await repo.GetUser(userId);
    }
}