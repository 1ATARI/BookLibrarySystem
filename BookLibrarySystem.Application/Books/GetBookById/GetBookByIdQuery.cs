using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Application.Books.GetBookById;

public record GetBookByIdQuery(Guid BookId) : IQuery<Book>;