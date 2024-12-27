using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Domain.Books;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(Guid bookId , CancellationToken cancellationToken);
    Task<IEnumerable<Book>> GetAllAsync( CancellationToken cancellationToken);
    Task<Book> AddAsync(Book book , CancellationToken cancellationToken);
    Task<Book> UpdateAsync(Book book , CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid bookId , CancellationToken cancellationToken);
    Task<bool> ExistsAsync(Guid bookId , CancellationToken cancellationToken);
}