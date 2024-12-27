using BookLibrarySystem.Domain.Abstraction;
using BookLibrarySystem.Domain.Books;
using BookLibrarySystem.Domain.Users;
using BookLibrarySystem.Domain.UsersBooks.Events;

namespace BookLibrarySystem.Domain.UsersBooks
{
    public sealed class UserBook : Entity
    {        

        private UserBook(
            Guid id,
            Guid userId,
            Guid bookId,
            DateTime borrowedDate,
            DateTime? returnedDate)
            : base(id)
        {
            if (userId == Guid.Empty) throw new ArgumentException("Invalid user ID.", nameof(userId));
            if (bookId == Guid.Empty) throw new ArgumentException("Invalid book ID.", nameof(bookId));
            if (borrowedDate > DateTime.Now) throw new ArgumentException("Borrowed date cannot be in the future.", nameof(borrowedDate));
            if (returnedDate.HasValue && returnedDate < borrowedDate) throw new ArgumentException("Returned date cannot be earlier than borrowed date.", nameof(returnedDate));

            UserId = userId;
            BookId = bookId;
            BorrowedDate = borrowedDate;
            ReturnedDate = returnedDate;
        }

        private UserBook() { } // For EF Core

        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public Guid BookId { get; private set; }
        public Book Book { get; private set; }

        public DateTime BorrowedDate { get; private set; }
        public DateTime? ReturnedDate { get; private set; }

        public bool IsReturned => ReturnedDate.HasValue;

        public static UserBook Borrow(Guid userId, Guid bookId, DateTime borrowedDate)
        {
            return new UserBook(
                Guid.NewGuid(),
                userId,
                bookId,
                borrowedDate,
                null);
        }
        public Result UpdateBorrowedDate(DateTime borrowedDate)
        {
            if (borrowedDate > DateTime.Now)
            {
                return Result.Failure(UserBookErrors.InvalidBorrowedDate);
            }

            BorrowedDate = borrowedDate;
            return Result.Success();
        }
        public Result Return(DateTime returnedDate)
        {
            if (IsReturned)
            {
                return Result.Failure(UserBookErrors.AlreadyReturned);
            }
            if (returnedDate < BorrowedDate)
            {
                return Result.Failure(UserBookErrors.InvalidReturnDate);
            }

            ReturnedDate = returnedDate;

            RaiseDomainEvent(new UserBookReturnedDomainEvent(Id, UserId, BookId, ReturnedDate.Value));

            return Result.Success();
        }
        
        



   
    }
}
