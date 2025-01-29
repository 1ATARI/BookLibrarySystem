namespace BookLibrarySystem.Application.Users.RegisterUser;

public record RegisterUserCommandDTO(
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Email,
    string Username,
    string Password);