using BookLibrarySystem.Application.Abstractions.Messaging;

namespace BookLibrarySystem.Application.Books.AddGenre;

public sealed record AddGenreToBookCommand(Guid BookId, Guid GenreId) : ICommand;
