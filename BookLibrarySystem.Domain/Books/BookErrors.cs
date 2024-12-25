using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Domain.Books;

public static class BookErrors
{
    public static Error BookAlreadyUnavailable = new(
        "Book.Available",
        "The Book with the specified identifier was not Available");
    
    public static Error InvalidGenre = new(
        "Book.Genre",
        "The Genre for this book is invalid");
    public static Error DuplicateGenre = new(
        "Book.Duplicate",
        "The Genre for this book already exists");

}