using FluentValidation;

namespace BookLibrarySystem.Application.UsersBooks.BorrowUserBook;

public class BorrowBookCommandValidator : AbstractValidator<BorrowBookCommand>
{
    public BorrowBookCommandValidator()
    {
        RuleFor(c => c.BorrowBookRequest.UserId)
            .NotEmpty().WithMessage("User ID must not be empty.") .OverridePropertyName("UserID"); 

        RuleFor(c => c.BorrowBookRequest.BookId)
            .NotEmpty().WithMessage("Book ID must not be empty.") .OverridePropertyName("BookID") ;

        RuleFor(c => c.BorrowBookRequest.BorrowedDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Borrowed date cannot be in the future.") .OverridePropertyName("BorrowedDate");
    }
}