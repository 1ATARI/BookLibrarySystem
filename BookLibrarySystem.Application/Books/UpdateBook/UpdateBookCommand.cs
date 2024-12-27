using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Books;

namespace BookLibrarySystem.Application.Books.UpdateBook;

public sealed record UpdateBookCommand(Guid BookId, string Title, string Description , DateTime PublicationDate ,int Pages , bool IsAvailable , Guid AuthorId) : ICommand<Book>;
