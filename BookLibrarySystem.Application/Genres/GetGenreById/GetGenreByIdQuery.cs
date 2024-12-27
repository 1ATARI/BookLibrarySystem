using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Genres;

namespace BookLibrarySystem.Application.Genres.GetGenreById;

public record GetGenreByIdQuery(Guid GenreId) : IQuery<Genre>;