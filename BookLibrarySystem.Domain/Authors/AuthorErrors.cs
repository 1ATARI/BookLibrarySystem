using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Domain.Authors;

public class AuthorErrors
{
    public static Error NotFound = new(
            "Author.NotFound",
            "Author not found.");
    public static Error InvalidAuthorId = new(
            "Author.InvalidAuthorId",
            "Invalid Autor Id.");
    public static Error AuthorAlreadyExists = new(
            "Author.AlreadyExists",
            "An author with the same name already exists.");
    public static Error BookAlreadyAssigned = new(
            "Author.BookAssigned",
            "This book is already assigned to the author.");
    public static Error CannotDeleteAuthorWithBooks = new(
        "Author.CannotDeleteWithBooks",
            "Cannot delete author with associated books.");
    public static Error DeleteFailed = new(
        "Author.DeleteFailed",
        "Failed to delete the author.");
    public static Error Overlap = new(
            "UserBook.Overlap",
            "A concurrency conflict occurred. Please retry..");
}