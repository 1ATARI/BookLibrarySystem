using BookLibrarySystem.Domain.BooksGenres;
using Microsoft.EntityFrameworkCore;

namespace BookLibrarySystem.Infrastructure.Repositories;

public class BookGenreRepository :Repository<BookGenre> ,  IBookGenreRepository
{
    public BookGenreRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<IEnumerable<BookGenre?>> GetByBookIdAsync(Guid bookId, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<BookGenre>().Where(b=>b.BookId == bookId).ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BookGenre>> GetByGenreIdAsync(Guid genreId, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<BookGenre>().Where(b=>b.BookId == genreId).ToListAsync(cancellationToken);
    }
}