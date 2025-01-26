using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Domain.Genres;

public static class GenreErrors
{
    
    public static Error GenreAlreadyUnavailable = new(
        "Genre.Available",
        "The Genre with the specified identifier was not Available");
    
    public static Error InvalidGenre = new(
        "Genre.Genre",
        "The Genre is invalid");
    public static Error DuplicateGenre = new(
        "Genre.Duplicate",
        "The Genre for this Book already exists");
    public static Error NotFound = new(
        "Genre.NotFound",
        "The Genre Not Found");
    public static Error DeleteFailed = new(
        "Genre.DeleteFailed",
        "Failed to delete the Genre with the specified identifier.");
    public static Error InvalidName = new(
        "Genre.InvalidName",
        "The Genre Name is invalid.");
    public static Error InvalidDescription = new(
        "Genre.InvalidName",
        "The Genre Description is invalid.");
    public static Error Overlap = new(
        "UserBook.Overlap",
        "A concurrency conflict occurred. Please retry..");

}