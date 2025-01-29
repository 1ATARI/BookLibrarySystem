using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Application.Users.LoginUser;

public sealed record LoginUserCommand(LoginUserRequestDTO LoginUserRequestDTO) : ICommand<LoginUserResponse>;



public sealed record LoginUserRequestDTO(string UserName, string Password);
