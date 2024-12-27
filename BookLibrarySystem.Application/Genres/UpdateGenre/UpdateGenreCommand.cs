using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Genres;

namespace BookLibrarySystem.Application.Genres.UpdateGenre;

public sealed record UpdateGenreCommand(Guid Id, string Name, string Description) : ICommand<Genre>;