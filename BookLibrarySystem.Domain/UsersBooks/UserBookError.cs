using BookLibrarySystem.Domain.Abstraction;

namespace BookLibrarySystem.Domain.UsersBooks;

public class UserBookError
{
    public static Error AlreadyReturned = new(     "BookReturn.AlreadyReturned",
        "the book Already Returned");
    
    public static Error InvalidReturnDate = new(     "BookReturn.InvalidReturnDate",
        "the Invalid Return Date for returning this book ");

}