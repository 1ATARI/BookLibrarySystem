using BookLibrarySystem.Application.Abstractions.Identity;
using BookLibrarySystem.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace BookLibrarySystem.Infrastructure.ForIdentity;


public class UserManager : IUserManager
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserManager(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<ApplicationUser?> FindByNameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _userManager.FindByNameAsync(username);
    }
    public async Task<ApplicationUser?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<IdentityResult> CreateAsync(ApplicationUser user, string password, CancellationToken cancellationToken = default)
    {
        return await _userManager.CreateAsync(user, password);
    }
}