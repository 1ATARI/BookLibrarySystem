using System.Linq.Expressions;
using BookLibrarySystem.Domain.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace BookLibrarySystem.Infrastructure.Repositories;

public abstract class Repository<T> : IRepository<T> where T : class, IIdentifiable
{
    protected readonly ApplicationDbContext _dbContext;

    protected Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    public async Task<T?> GetByIdAsync(
        Guid id,
        string? includeProperties = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = IncludeProperties(includeProperties);

        return await query
            .FirstOrDefaultAsync(entity => entity.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null,
        int skip = 0,
        int take = 10,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = IncludeProperties(includeProperties);

        if (filter != null)
        {
            query = query.Where(filter);
        }
        query = query.OrderBy(a => a.Id);

        query = query
            .Skip(skip)
            .Take(take);
        return await query.AsNoTracking().ToListAsync(cancellationToken);

    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>()
            .AnyAsync(a => a.Id == id, cancellationToken);
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
    }

    public void Update(T entity ,CancellationToken cancellationToken = default)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        _dbContext.Set<T>().Attach(entity);
        _dbContext.Entry(entity).State = EntityState.Modified;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken: cancellationToken);
        if (entity == null)
        {
            return false;
        }

        _dbContext.Set<T>().Remove(entity);
        return true;
    }

    private IQueryable<T> IncludeProperties(string? includeProperties)
    {
        IQueryable<T> query = _dbContext.Set<T>();

        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProperty in includeProperties.Split(new[] { ',' },
                         StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }
        }

        return query;
    }
}