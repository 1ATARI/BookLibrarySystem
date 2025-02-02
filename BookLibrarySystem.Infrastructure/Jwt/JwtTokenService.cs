using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookLibrarySystem.Application.Abstractions.Identity;
using BookLibrarySystem.Application.Abstractions.JWT;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BookLibrarySystem.Infrastructure.Jwt;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;

    public JwtTokenService(IOptions<JwtSettings> jwtSettings)
    {
        _jwtSettings = jwtSettings.Value;

        if (string.IsNullOrEmpty(_jwtSettings.Key))
        {
            throw new InvalidOperationException("JWT secret key is not configured.");
        }

        if (string.IsNullOrEmpty(_jwtSettings.Issuer))
        {
            throw new InvalidOperationException("JWT issuer is not configured.");
        }

        if (string.IsNullOrEmpty(_jwtSettings.Audience))
        {
            throw new InvalidOperationException("JWT audience is not configured.");
        }
    }

    public Result<string> GenerateToken(ApplicationUser user)
    {
        try
        {
            // Validate user input
            if (user == null)
            {
                return Result.Failure<string>(new Error("Jwt.InvalidUser", "User cannot be null."));
            }

            var keyBytes = Encoding.UTF8.GetBytes(_jwtSettings.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("FirstName", user.Name.FirstName),
                    new Claim("LastName", user.Name.LastName)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Result.Success(tokenHandler.WriteToken(token));
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(JwtErrors.TokenGenerationFailed);
        }
    }
}