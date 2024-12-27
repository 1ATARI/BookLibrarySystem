namespace BookLibrarySystem.Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid userId , CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetAllAsync(  CancellationToken cancellationToken);
    Task<User> AddAsync(User user , CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid userId , CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid userId , CancellationToken cancellationToken);
}