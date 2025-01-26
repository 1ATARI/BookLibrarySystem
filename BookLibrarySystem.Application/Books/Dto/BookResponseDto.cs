namespace BookLibrarySystem.Application.Books.Dto;

public record BookResponseDto(
    Guid Id,
    string Title,
    string Description,
    DateTime PublicationDate,
    int Pages,
    bool IsAvailable,
    List<GenreDto> Genres,
    AuthorDto Author
);