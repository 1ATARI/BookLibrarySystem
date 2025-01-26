using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Domain.Authors
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task AddBookToAuthorAsync(Guid authorId, Book book , CancellationToken cancellationToken = default);
    }
    
}