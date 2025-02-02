using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Users;

namespace BookLibrarySystem.Application.Abstractions.JWT;

public interface IJwtTokenService
{
    Result<string> GenerateToken(ApplicationUser user);
}