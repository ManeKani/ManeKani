public interface IUserRepository
{
    public Task<bool> IsUserComplete(Guid userId);
}