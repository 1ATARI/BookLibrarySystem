using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Application.Users.DeleteUser;

public sealed record DeleteUserCommand(Guid UserId) : ICommand<Result>;