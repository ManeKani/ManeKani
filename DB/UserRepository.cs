
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
}