using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Application.Books.DeleteBook;

public record DeleteBookCommand(Guid BookId) : ICommand;


