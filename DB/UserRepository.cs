
using SqlKata.Execution;

namespace ManeKani.DB;

public class UserRepository : Repository, IUserRepository
{
    struct SelectIsComplete
    {
        public bool IsComplete;
    }

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