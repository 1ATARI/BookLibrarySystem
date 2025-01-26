using BookLibrarySystem.Domain.Authors;
using BookLibrarySystem.Domain.Books;
using Microsoft.EntityFrameworkCore;

namespace BookLibrarySystem.Infrastructure.Repositories;

internal sealed class AuthorRepository : Repository<Author> ,  IAuthorRepository
{
    public AuthorRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    

    public async Task AddBookToAuthorAsync(Guid authorId, Book book , CancellationToken cancellationToken)
    {
        var author = await GetByIdAsync(authorId,null, cancellationToken);
        if (author == null)
        {
            throw new KeyNotFoundException("Author not found");
        }
        author.AddBook(book);
    }
}