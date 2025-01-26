using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Domain.BooksGenres;

public interface IBookGenreRepository : IRepository<BookGenre>
{
    Task<IEnumerable<BookGenre?>> GetByBookIdAsync(Guid bookId , CancellationToken cancellationToken);
}