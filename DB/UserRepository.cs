
using ManeKani.Core.Interfaces;
using ManeKani.Core.Models;
using SqlKata.Execution;

namespace ManeKani.DB;

public class UserRepository : Repository, IUserRepository
{
    public Task<bool> IsUserComplete(Guid userId)
    {
        return Database.Query("users")
            .Where(new
            {
                id = userId,
                is_complete = true,
            })
            .ExistsAsync();
    }

    public Task<User> GetUser(Guid userId)
    {
        return Database.Query("users")
            .Where(new
            {
                id = userId,
            })
            .FirstOrDefaultAsync<User>();
    }
}