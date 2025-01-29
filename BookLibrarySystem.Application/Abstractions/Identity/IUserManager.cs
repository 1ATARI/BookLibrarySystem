using BookLibrarySystem.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace BookLibrarySystem.Application.Abstractions.Identity;

public interface IUserManager
{
    Task<ApplicationUser?> FindByNameAsync(string username, CancellationToken cancellationToken = default);
    Task<ApplicationUser?> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<IdentityResult> CreateAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default);

}