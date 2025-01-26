using System.Linq.Expressions;

namespace BookLibrarySystem.Domain.Abstraction;

public interface IRepository <T> where T : IIdentifiable
{
    
    Task<T?> GetByIdAsync(Guid entityId, string? includeProperties = null, CancellationToken cancellationToken = default);
    
    Task<IEnumerable<T>> GetAllAsync(
        Expression<Func<T, bool>>? filter = null,
        string? includeProperties = null,
        int skip = 0,
        int take = 10,
        CancellationToken cancellationToken = default);
    
    Task AddAsync(T entity , CancellationToken cancellationToken =default );
    void Update(T entity  ,CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid entityId , CancellationToken cancellationToken);
    Task<bool> ExistsByIdAsync(Guid entityId , CancellationToken cancellationToken);
}
