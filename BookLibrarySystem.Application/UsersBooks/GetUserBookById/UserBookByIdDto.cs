namespace BookLibrarySystem.Application.UsersBooks.GetUserBookById;

public record UserBookByIdDto(
    Guid UserBookId,
    Guid UserId,
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    Guid BookId,
    string BookTitle
);