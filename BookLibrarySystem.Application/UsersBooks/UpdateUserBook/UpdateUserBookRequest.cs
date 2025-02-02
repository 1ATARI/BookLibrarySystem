namespace BookLibrarySystem.Application.UsersBooks.UpdateUserBook;

public sealed record UpdateUserBookRequest(Guid UserId, Guid BookId, DateTime BorrowedDate, DateTime? ReturnedDate);