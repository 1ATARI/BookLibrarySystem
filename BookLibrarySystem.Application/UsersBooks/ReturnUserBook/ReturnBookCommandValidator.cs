using FluentValidation;

namespace BookLibrarySystem.Application.UsersBooks.ReturnUserBook;

public class ReturnBookCommandValidator : AbstractValidator<ReturnBookCommand>
{
    public ReturnBookCommandValidator()
    {
        RuleFor(c => c.UserBookId)
            .NotEmpty()
            .WithMessage("UserBook ID must not be empty.");

        RuleFor(c => c.ReturnedDate)
            .NotEmpty()
            .WithMessage("Returned date must not be empty.")
            .LessThanOrEqualTo(DateTime.Now)
            .WithMessage("Returned date cannot be in the future.");
    }
}