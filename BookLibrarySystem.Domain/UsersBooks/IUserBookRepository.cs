namespace BookLibrarySystem.Domain.UsersBooks;

public interface IUserBookRepository
{
    Task<UserBook?>GetByIdAsync(Guid userBookId , CancellationToken cancellationToken);
    Task<IEnumerable<UserBook>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<UserBook>> GetByUserIdAsync(Guid userId,CancellationToken cancellationToken);
    Task<IEnumerable<UserBook>> GetByBookIdAsync(Guid bookId,CancellationToken cancellationToken);
    Task<UserBook> AddAsync(UserBook userBook,CancellationToken cancellationToken);
    Task<UserBook> UpdateAsync(UserBook userBook,CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid userBookId,CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid userBookId,CancellationToken cancellationToken);
}