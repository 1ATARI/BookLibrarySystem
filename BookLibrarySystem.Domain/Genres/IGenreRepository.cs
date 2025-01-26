using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Domain.Genres;

public interface IGenreRepository : IRepository<Genre>
{
    Task<bool> ExistsByNameAsync(Name name , CancellationToken cancellationToken);
}