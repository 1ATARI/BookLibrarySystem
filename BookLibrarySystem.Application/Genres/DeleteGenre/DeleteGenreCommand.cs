using BookLibrarySystem.Application.Abstractions.Messaging;

namespace BookLibrarySystem.Application.Genres.DeleteGenre;

public sealed record DeleteGenreCommand(Guid Id) : ICommand;