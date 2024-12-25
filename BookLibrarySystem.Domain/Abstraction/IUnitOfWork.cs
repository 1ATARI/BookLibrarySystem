namespace BookLibrarySystem.Domain.Abstraction;

public interface IUnitOfWork
{
    Task <int> SaveChangesAsync(CancellationToken cancellationToken = default);
}