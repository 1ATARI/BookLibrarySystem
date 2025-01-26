using FluentValidation;

namespace BookLibrarySystem.Application.Authors.DeleteAuthor;

public class DeleteAuthorCommandValidator : AbstractValidator<DeleteAuthorCommand>
{
    public DeleteAuthorCommandValidator()
    {
        RuleFor(x => x.AuthorId)
            .NotEmpty().WithMessage("Author ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Author ID must not be empty.");
    }
}