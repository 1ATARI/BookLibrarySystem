namespace BookLibrarySystem.Application.Authors.GetAllAuthors;

public record AuthorResponseDto(
    Guid Id,
    string FirstName,
    string LastName,
    List<BookResponseDto> Books);