namespace BookLibrarySystem.Application.UsersBooks.GetUserBooksByBookId;

public record UserBookByBookIdDto(
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