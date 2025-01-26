namespace BookLibrarySystem.Application.Books.UpdateBook;

public record UpdateBookDto(
    string Title,
    string Description,
    DateTime PublicationDate,
    int Pages,
    Guid AuthorId,
    bool IsAvailable,
    List<Guid> GenreIds
    );