namespace BookLibrarySystem.Application.Books.CreateBooks;

public record CreateBookDto(
    string Title,
    string Description,
    DateTime PublicationDate,
    int Pages,
    Guid AuthorId,
    bool IsAvailable,
    List<Guid> GenreIds
);