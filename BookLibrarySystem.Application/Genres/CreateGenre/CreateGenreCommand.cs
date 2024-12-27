using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Genres;

namespace BookLibrarySystem.Application.Genres.CreateGenre;

public sealed record CreateGenreCommand(string Name, string Description) : ICommand<Genre>;