using BookLibrarySystem.Application.Abstractions.Identity;
using BookLibrarySystem.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace BookLibrarySystem.Infrastructure.ForIdentity;

public class SignInManager : ISignInManager
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public SignInManager(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }

    public async Task<SignInResult> CheckPasswordSignInAsync(ApplicationUser user, string password, bool lockoutOnFailure , CancellationToken cancellationToken)
    {
        return await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
    }
}