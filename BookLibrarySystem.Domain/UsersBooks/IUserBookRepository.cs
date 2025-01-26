using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Domain.UsersBooks;

public interface IUserBookRepository : IRepository<UserBook>
{

    Task<IEnumerable<UserBook>> GetByUserIdAsync(Guid userId,CancellationToken cancellationToken);
    Task<IEnumerable<UserBook>> GetByBookIdAsync(Guid bookId,CancellationToken cancellationToken);
}