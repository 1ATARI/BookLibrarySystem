using BookLibrarySystem.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace BookLibrarySystem.Application.Abstractions.Identity;

public interface ISignInManager
{
    Task<SignInResult> CheckPasswordSignInAsync(ApplicationUser user, string password, bool lockoutOnFailure, CancellationToken cancellationToken =default);
}