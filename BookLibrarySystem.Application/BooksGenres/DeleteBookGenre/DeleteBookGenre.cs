using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Application.BooksGenres.DeleteBookGenre;

public sealed record DeleteBookGenreCommand(Guid BookGenreId) : ICommand;