namespace BookLibrarySystem.Application.UsersBooks.GetAllUserBook;

public record UserBookDto(
    Guid UserBookId,
    Guid UserId,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    Guid BookId,
    string BookTitle,
    DateTime? ReturnDate
);