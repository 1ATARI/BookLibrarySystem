using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.UsersBooks;

namespace BookLibrarySystem.Application.UsersBooks.UpdateUserBook;

public record UpdateUserBookCommand(Guid UserBookId, DateTime BorrowedDate, DateTime? ReturnedDate) : ICommand<UserBook>;