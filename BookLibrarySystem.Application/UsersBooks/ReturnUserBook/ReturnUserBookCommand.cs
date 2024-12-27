using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Application.UsersBooks.ReturnUserBook;

public record ReturnBookCommand(Guid UserBookId, DateTime ReturnedDate) : ICommand<Result>;
