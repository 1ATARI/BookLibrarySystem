using FluentValidation;

namespace BookLibrarySystem.Application.Authors.UpdateAuthor;

public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
{
    public UpdateAuthorCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Author ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Author ID must not be empty.");
    }
}