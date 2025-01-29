using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BookLibrarySystem.Application.Abstractions.JWT;
using BookLibrarySystem.Domain.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BookLibrarySystem.Infrastructure.Jwt;

public class JwtTokenService : IJwtTokenService
{

        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = jwtSettings["Key"];

            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT secret key is not configured.");
            }

            var keyBytes = Encoding.UTF8.GetBytes(key); 

            var expiryInMinutesString = jwtSettings["ExpiryInMinutes"];
            int expiryInMinutes = 60;  

            if (!string.IsNullOrEmpty(expiryInMinutesString) && int.TryParse(expiryInMinutesString, out var parsedExpiry))
            {
                expiryInMinutes = parsedExpiry;  
            }
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                        new Claim("UserId",  user.Id.ToString()),
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName ?? string.Empty),
                        
                    new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("FirstName", user.Name.FirstName),
                    new Claim("LastName", user.Name.LastName)
                }),
                
                Expires = DateTime.UtcNow.AddMinutes(expiryInMinutes),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
      
}