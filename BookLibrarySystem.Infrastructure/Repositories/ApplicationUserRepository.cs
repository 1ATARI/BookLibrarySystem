using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Users;

namespace BookLibrarySystem.Infrastructure.Repositories;

internal sealed class ApplicationUserRepository :Repository<ApplicationUser> , IApplicationUserRepository
{
    public ApplicationUserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }
    
}