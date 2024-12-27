using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Domain.BooksGenres;

public static class BookGenreErrors
{
    public static readonly Error InvalidBookId = new(
        "BookGenre.InvalidBookId",
        "The Book ID cannot be empty.");
    public static readonly Error InvalidGenreId = new(
        "BookGenre.InvalidGenreId",
        "The Genre ID cannot be empty.");
    public static readonly Error DuplicateGenre = new(
        "BookGenre.Duplicate",
        "The genre already exists for this book.");
    public static readonly Error NotFound = new(
        "BookGenre.NotFound",
        "The specified BookGenre was not found.");
    public static Error Overlap = new(
        "UserBook.Overlap",
        "A concurrency conflict occurred. Please retry..");
}