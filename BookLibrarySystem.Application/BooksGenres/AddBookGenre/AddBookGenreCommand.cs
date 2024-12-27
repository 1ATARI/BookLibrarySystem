using BookLibrarySystem.Application.Abstractions.Messaging;

namespace BookLibrarySystem.Application.BooksGenres.AddBookGenre;

public sealed record AddBookGenreCommand(Guid BookId, Guid GenreId) : ICommand;