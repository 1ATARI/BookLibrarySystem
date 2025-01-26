using BookLibrarySystem.Domain.Books;
using Microsoft.EntityFrameworkCore;

namespace BookLibrarySystem.Infrastructure.Repositories;

internal sealed class BookRepository :Repository<Book>, IBookRepository
{
    public BookRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    // public async Task<IEnumerable<Book>> GetAllAsync(CancellationToken cancellationToken = default)
    // {
    //     return await DbContext.Set<Book>()
    //         .Include(b => b.Genres)
    //         .ThenInclude(bg => bg.Genre)
    //         .Include(b => b.Author)
    //         .ToListAsync(cancellationToken);
    // }
    //



}
