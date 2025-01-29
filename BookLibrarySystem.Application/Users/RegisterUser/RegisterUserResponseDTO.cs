namespace BookLibrarySystem.Application.Users.RegisterUser;

public record RegisterUserResponseDTO(
    Guid Id,
    string FirstName,
    string LastName,
     string Email,
    string Username,
    DateTime DateOfBirth
);