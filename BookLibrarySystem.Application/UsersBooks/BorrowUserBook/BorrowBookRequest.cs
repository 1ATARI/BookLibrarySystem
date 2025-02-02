namespace BookLibrarySystem.Application.UsersBooks.BorrowUserBook;

public record BorrowBookRequest(Guid UserId, Guid BookId, DateTime BorrowedDate , DateTime ReturnDate);