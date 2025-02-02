using System.Text;
using BookLibrarySystem.Application.Abstractions.Email;
using BookLibrarySystem.Application.Abstractions.Identity;
using BookLibrarySystem.Application.Abstractions.JWT;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Authors;
using BookLibrarySystem.Domain.Books;
using BookLibrarySystem.Domain.BooksGenres;
using BookLibrarySystem.Domain.Genres;
using BookLibrarySystem.Domain.Users;
using BookLibrarySystem.Domain.UsersBooks;
using BookLibrarySystem.Infrastructure.Email;
using BookLibrarySystem.Infrastructure.ForIdentity;
using BookLibrarySystem.Infrastructure.Jwt;
using BookLibrarySystem.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace BookLibrarySystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new NullReferenceException(nameof(configuration));

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IBookGenreRepository, BookGenreRepository>();
        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IUserBookRepository, UserBookRepository>();
        services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<ISignInManager, SignInManager>();
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<IJwtTokenService,JwtTokenService>();
        services.Configure<MailSettings>(configuration.GetSection("Mail"));
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

        services.AddTransient<IEmailService, EmailService>();

        services.AddScoped<IUnitOfWork>(sp=>sp.GetRequiredService<ApplicationDbContext>());

        var jwtSettings = configuration.GetSection("Jwt");
        
        var key = jwtSettings["Key"];

        if (string.IsNullOrEmpty(key))
        {
            throw new InvalidOperationException("JWT secret key is not configured.");
        }

        var keyBytes = Encoding.UTF8.GetBytes(key); 

        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                };
            });
        
        return services;
    }
}