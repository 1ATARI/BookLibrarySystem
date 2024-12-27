using BookLibrarySystem.Application.Abstractions.Messaging;
using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Users;

namespace BookLibrarySystem.Application.Users.RegisterUser;

public record RegisterUserCommand (string firstName, string lastName, DateTime dateOfBirth, string email, string username, string password):ICommand<Result<User>>;