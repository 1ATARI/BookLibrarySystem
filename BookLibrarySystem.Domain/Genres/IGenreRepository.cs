using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Domain.Genres;

public interface IGenreRepository
{
    Task <Genre?> GetByIdAsync(Guid genreId , CancellationToken cancellationToken );
    Task<IEnumerable<Genre>> GetAllAsync( CancellationToken cancellationToken);
    Task<Book> AddAsync(Genre genre , CancellationToken cancellationToken);
    Task<Book> UpdateAsync(Genre genre , CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Guid genreId , CancellationToken cancellationToken);
    Task<bool> ExistsByIdAsync(Guid genreId , CancellationToken cancellationToken);
    Task<bool> ExistsByNameAsync(Name name , CancellationToken cancellationToken);
}