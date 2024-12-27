using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Domain.Authors
{
    public interface IAuthorRepository
    {
        Task AddAsync(Author author , CancellationToken cancellationToken);
        Task<Author?> GetByIdAsync(Guid id,  CancellationToken cancellationToken);
        Task<IEnumerable<Author>> GetAllAsync( CancellationToken cancellationToken);
        Task<bool> ExistsAsync(Guid id ,  CancellationToken cancellationToken);
        Task UpdateAsync(Author author, CancellationToken cancellationToken);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task AddBookToAuthorAsync(Guid authorId, Book book);
    }
    
}