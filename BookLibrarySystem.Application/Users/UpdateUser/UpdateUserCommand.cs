using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Application.Users.UpdateUser;

public sealed record UpdateUserCommand(Guid UserId,string? FirstName,string? LastName,string? Email,string? Username) : ICommand<Result>;