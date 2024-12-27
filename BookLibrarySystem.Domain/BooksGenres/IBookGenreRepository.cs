namespace BookLibrarySystem.Domain.BooksGenres;

public interface IBookGenreRepository
{
    Task<BookGenre?> GetByIdAsync(Guid bookGenreId , CancellationToken cancellationToken);
    Task<IEnumerable<BookGenre>> GetAllAsync(CancellationToken cancellationToken);
    Task<IEnumerable<BookGenre?>> GetByBookIdAsync(Guid bookId , CancellationToken cancellationToken);
    Task<IEnumerable<BookGenre>> GetByGenreIdAsync(Guid genreId , CancellationToken cancellationToken);
    Task AddAsync(BookGenre bookGenre , CancellationToken cancellationToken);
    Task DeleteAsync(Guid bookGenreId , CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid bookGenreId , CancellationToken cancellationToken);
}