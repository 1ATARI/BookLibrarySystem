using FluentValidation;

namespace BookLibrarySystem.Application.UsersBooks.UpdateUserBook;

public class UpdateUserBookCommandValidator : AbstractValidator<UpdateUserBookCommand>
{
    public UpdateUserBookCommandValidator()
    {
        RuleFor(c => c.UserBookId)
            .NotEmpty().WithMessage("UserBook ID must not be empty.");

        RuleFor(c => c.BorrowedDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Borrowed date cannot be in the future.");

        RuleFor(c => c.ReturnedDate)
            .GreaterThanOrEqualTo(c => c.BorrowedDate)
            .When(c => c.ReturnedDate.HasValue)
            .WithMessage("Returned date cannot be earlier than borrowed date.");
    }
}