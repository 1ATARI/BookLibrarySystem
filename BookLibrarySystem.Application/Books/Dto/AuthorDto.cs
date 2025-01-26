namespace BookLibrarySystem.Application.Books.Dto;

public record AuthorDto(
    Guid Id,
    string FirstName,
    string LastName
);