using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Application.Users.UpdateUser;

public sealed record UpdateUserCommand(Guid UserId,UpdateUserDto UserDto) : ICommand;

public sealed record UpdateUserDto( 
    string? FirstName, 
    string? LastName, 
    string? Email, 
    string? Username, 
    string? PhoneNumber, 
    string? Password
);