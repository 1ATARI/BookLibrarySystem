using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.BooksGenres;

namespace BookLibrarySystem.Application.BooksGenres.UpdateBookGenre;

    public sealed record UpdateGenreForBookCommand(Guid BookId, Guid OldGenreId, Guid NewGenreId) : ICommand;
