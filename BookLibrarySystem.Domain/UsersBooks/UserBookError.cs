using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Domain.UsersBooks;

public class UserBookErrors
{
    public static Error AlreadyReturned = new(     "BookReturn.AlreadyReturned",
        "the book Already Returned");
    
    public static Error InvalidReturnDate = new(     "BookReturn.InvalidReturnDate",
        "the Invalid Return Date for returning this book ");
    
    public static Error UserNotFound = new(
        "UserBook.UserNotFound",
        "The user was not found.");

    public static Error BookUnavailable = new(
        "UserBook.BookUnavailable",
        "The book is not available for borrowing.");
    

        public static Error UserBookNotFound = new(
        "UserBook.UserBookNotFound",
        "The user-book relationship was not found.");
        
    public static Error InvalidBorrowedDate = new(
        "UserBook.InvalidBorrowedDate",
        "The borrowed date is valid.");
    
    public static Error Overlap = new(
        "UserBook.Overlap",
        "A concurrency conflict occurred. Please retry..");
    public static Error updateReturnedDateResult = new(
        "UserBook.updateReturnedDateResult",
        "Error happen while update returned Date");
}