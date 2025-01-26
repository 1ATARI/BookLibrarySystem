using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Application.Genres.Dto;
using BookLibrarySystem.Application.Genres.GetAllGenres;
using BookLibrarySystem.Domain.Genres;

namespace BookLibrarySystem.Application.Genres.GetGenreById;

public record GetGenreByIdQuery(Guid GenreId) : IQuery<GenreResponseDto>;