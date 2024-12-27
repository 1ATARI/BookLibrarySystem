using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Domain.Users;

public static class UserErrors
{
    public static Error NotFound = new(
        "User.UserNotFound",
        "The user was not found.");
    
    public static Error CreationFailed = new(
        "User.CreationFailed",
        "Failed to create the user.");
    public static Error Overlap = new(
        "UserBook.Overlap",
        "A concurrency conflict occurred. Please retry..");
}