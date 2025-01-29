using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Users;

namespace BookLibrarySystem.Application.Users.RegisterUser;

public sealed record RegisterUserCommand(
    RegisterUserCommandDTO CommandDTO ) :  ICommand<RegisterUserResponseDTO>;