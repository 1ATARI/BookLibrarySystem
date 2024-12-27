using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Domain.Books;

public static class BookErrors
{
    public static Error BookUnavailable = new(
        "Book.Available",
        "The Book not Available");

    public static Error InvalidGenre = new(
        "Book.Genre",
        "The Genre for this book is invalid");

    public static Error DuplicateGenre = new(
        "Book.Duplicate",
        "The Genre for this book already exists");

    public static Error NotFound = new(
        "Book.Found",
        "The Book Not Found");

    public static Error DeleteFailed = new(
        "Book.DeleteFailed",
        "Failed to delete the book with the specified identifier.");

    public static readonly Error BookNotFound = new(
        "Book.NotFound",
        "The specified book was not found.");

    public static readonly Error GenreNotFound = new(
        "Book.GenreNotFound",
        "The specified genre was not found.");

    public static readonly Error GenreNotAssociated = new(
        "Book.GenreNotAssociated",
        "The specified genre is not associated with this book.");

    public static readonly Error NewGenreNotFound = new(
        "BookGenre.NewGenreNotFound",
        "The specified new genre was not found.");

    public static Error BookAlreadyAvailable = new(
        "Book.AlreadyAvailable",
        "The book is already available.");

    public static Error Overlap = new(
        "UserBook.Overlap",
        "A concurrency conflict occurred. Please retry..");
}