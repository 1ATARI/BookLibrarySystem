using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Application.Books.UpdateBook;

public record UpdateBookCommand(Guid BookId, UpdateBookDto BookDto) : ICommand;
