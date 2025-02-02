using FluentValidation;

namespace BookLibrarySystem.Application.UsersBooks.UpdateUserBook;

public class UpdateUserBookCommandValidator : AbstractValidator<UpdateUserBookCommand>
{
    public UpdateUserBookCommandValidator()
    {
        RuleFor(c => c.UserBookId)
            .NotEmpty().WithMessage("UserBook ID must not be empty.").OverridePropertyName("UserBookID");;

        RuleFor(c => c.UserBookRequest.BorrowedDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Borrowed date cannot be in the future.").OverridePropertyName("BorrowedDate");;

        RuleFor(c => c.UserBookRequest.ReturnedDate)
            .GreaterThanOrEqualTo(c => c.UserBookRequest.BorrowedDate)
            .When(c => c.UserBookRequest.ReturnedDate.HasValue)
            .WithMessage("Returned date cannot be earlier than borrowed date.").OverridePropertyName("ReturnDate");;
    }
}