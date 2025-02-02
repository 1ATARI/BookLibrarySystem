using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Application.UsersBooks.DeleteUserBook;

public record DeleteUserBookCommand(Guid UserBookId) : ICommand;