using BookLibrarySystem.Domain.Genres;
using Microsoft.EntityFrameworkCore;

namespace BookLibrarySystem.Infrastructure.Repositories;

public class GenreRepository : Repository<Genre>, IGenreRepository
{
    public GenreRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<bool> ExistsByNameAsync(Name name, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Genre>().AnyAsync(e => e.Name.Value == name.Value, cancellationToken);
    }
}