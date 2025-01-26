using FluentValidation;

namespace BookLibrarySystem.Application.UsersBooks.CreateUserBook;

public class CreateUserBookCommandValidator : AbstractValidator<CreateUserBookCommand>
{
    public CreateUserBookCommandValidator()
    {
        RuleFor(c => c.UserId)
            .NotEmpty().WithMessage("User ID must not be empty.");

        RuleFor(c => c.BookId)
            .NotEmpty().WithMessage("Book ID must not be empty.");

        RuleFor(c => c.BorrowedDate)
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Borrowed date cannot be in the future.");
    }
}