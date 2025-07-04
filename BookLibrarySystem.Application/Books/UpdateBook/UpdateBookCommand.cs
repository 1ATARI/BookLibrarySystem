using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Application.Books.UpdateBook;

public record UpdateBookCommand(
    Guid BookId,
    string Title,
    string Description,
    DateTime PublicationDate,
    int Pages,
    Guid AuthorId,
    bool IsAvailable,
    List<Guid> GenreIds) : ICommand;