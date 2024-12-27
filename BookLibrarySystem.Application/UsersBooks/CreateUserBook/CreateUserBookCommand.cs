using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Application.UsersBooks.CreateUserBook;

public record CreateUserBookCommand(Guid UserId, Guid BookId, DateTime BorrowedDate) : ICommand<UserBook>;