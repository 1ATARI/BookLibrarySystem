using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Application.Abstractions.Identity;

public static class JwtErrors
{
    public static readonly Error InvalidConfiguration = new(
        "Jwt.InvalidConfiguration",
        "JWT configuration is invalid or missing.");

    public static readonly Error TokenGenerationFailed = new(
        "Jwt.TokenGenerationFailed",
        "Failed to generate JWT token.");
}