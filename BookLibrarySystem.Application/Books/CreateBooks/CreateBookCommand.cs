using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Application.Books.CreateBooks;

public record CreateBookCommand(CreateBookDto BookDto) : ICommand<Book>;