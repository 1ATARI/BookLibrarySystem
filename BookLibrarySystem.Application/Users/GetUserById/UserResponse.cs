namespace BookLibrarySystem.Application.Users.GetUserById;

public record UserResponse(Guid Id, string FirstName, string LastName, string Email, string Username);
