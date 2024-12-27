using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Application.UsersBooks.BorrowUserBook;

public record BorrowBookCommand(Guid UserId, Guid BookId, DateTime BorrowedDate) : ICommand<Guid>;
