namespace BookLibrarySystem.Application.Authors.GetAllAuthors;

public record BookResponseDto(
    Guid Id,
    string Title,
    string Description,
    DateTime PublicationDate,
    int Pages,
    bool IsAvailable);