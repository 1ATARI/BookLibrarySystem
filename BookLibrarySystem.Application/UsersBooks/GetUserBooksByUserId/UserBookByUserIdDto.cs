namespace BookLibrarySystem.Application.UsersBooks.GetUserBooksByUserId;

public record UserBookByUserIdDto(
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