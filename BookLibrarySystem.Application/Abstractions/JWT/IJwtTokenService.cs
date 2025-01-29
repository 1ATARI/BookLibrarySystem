using BookLibrarySystem.Domain.Users;

namespace BookLibrarySystem.Application.Abstractions.JWT;

public interface IJwtTokenService
{
    string GenerateToken(ApplicationUser user);
}