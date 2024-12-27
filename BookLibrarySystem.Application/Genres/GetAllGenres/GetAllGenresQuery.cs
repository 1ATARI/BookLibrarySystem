using BookLibrarySystem.Application.Abstractions.Messaging;

namespace BookLibrarySystem.Application.Genres.GetAllGenres;

public sealed record GetAllGenresQuery : IQuery<IEnumerable<GenreResponse>>;