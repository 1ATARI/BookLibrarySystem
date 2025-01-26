using FluentValidation;

namespace BookLibrarySystem.Application.UsersBooks.DeleteUserBook;

public class DeleteUserBookCommandValidator : AbstractValidator<DeleteUserBookCommand>
{
    public DeleteUserBookCommandValidator()
    {
        RuleFor(c => c.UserBookId)
            .NotEmpty().WithMessage("UserBook ID must not be empty.");
    }
}