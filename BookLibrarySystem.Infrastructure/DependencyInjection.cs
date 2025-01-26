using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Authors;
using BookLibrarySystem.Domain.Books;
using BookLibrarySystem.Domain.BooksGenres;
using BookLibrarySystem.Domain.Genres;
using BookLibrarySystem.Domain.Users;
using BookLibrarySystem.Domain.UsersBooks;
using BookLibrarySystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
        services.AddScoped<IUnitOfWork>(sp=>sp.GetRequiredService<ApplicationDbContext>());

        return services;
    }
}