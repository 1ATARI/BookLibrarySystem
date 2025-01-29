using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Domain.Users;

public static class ApplicationUserErrors
{
    public static Error NotFound = new(
        "User.UserNotFound",
        "The user was not found.");
    public static Error EmailExists = new(
        "User.EmailAlreadyExists",
        "The email is already in use by another user.");
    public static Error UsernameExists = new(
        "User.UsernameAlreadyExists",
        "The username is already in use by another user.");
    
    public static Error CreationFailed = new(
        "User.CreationFailed",
        "Failed to create the user.");
    public static Error Overlap = new(
        "UserBook.Overlap",
        "A concurrency conflict occurred. Please retry..");
}