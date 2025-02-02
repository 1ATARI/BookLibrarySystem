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
        try
        {
            return await _dbContext.Set<UserBook>()
                .Include(ub => ub.ApplicationUser) // Include ApplicationUser
                .Include(ub => ub.Book) // Include Book
                .Where(e => e.UserId == userId)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {

            throw new ApplicationException($"An error occurred while fetching UserBook records for user {userId}.", ex);
        }
    
    }

    public async Task<IEnumerable<UserBook>> GetByBookIdAsync(Guid bookId, CancellationToken cancellationToken)
    {
        try
        {
            return await _dbContext.Set<UserBook>()
                .Include(ub => ub.ApplicationUser) 
                .Include(ub => ub.Book) 
                .Where(e => e.BookId == bookId)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"An error occurred while fetching UserBook records for book {bookId}.", ex);
        }    }
}