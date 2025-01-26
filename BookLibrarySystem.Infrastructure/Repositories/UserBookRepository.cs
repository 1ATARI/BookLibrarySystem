using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.UsersBooks;
using Microsoft.EntityFrameworkCore;

namespace BookLibrarySystem.Infrastructure.Repositories;

internal sealed class UserBookRepository :Repository<UserBook > , IUserBookRepository
{
    public UserBookRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<UserBook>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<UserBook>().Where(e=>e.UserId== userId).ToListAsync(cancellationToken);

    }

    public async Task<IEnumerable<UserBook>> GetByBookIdAsync(Guid bookId, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<UserBook>().Where(e=>e.BookId == bookId).ToListAsync(cancellationToken);
    }
}