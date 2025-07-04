using FluentValidation;

namespace BookLibrarySystem.Application.Authors.AddBookToAuthor;

public class AddBookToAuthorCommandValidator : AbstractValidator<AddBookToAuthorCommand>
{
public AddBookToAuthorCommandValidator()
{
    RuleFor(x => x.AuthorId)
        .NotEmpty().WithMessage("Author ID is required.");

    RuleFor(x => x.Title)
        .NotEmpty().WithMessage("Book title is required.");

    RuleFor(x => x.Description)
        .NotEmpty().WithMessage("Book description is required.");

    RuleFor(x => x.PublicationDate)
        .NotEqual(default(DateTime)).WithMessage("Publication date is required.")
        .LessThanOrEqualTo(DateTime.Now).WithMessage("Publication date cannot be in the future.");

    RuleFor(x => x.Pages)
        .GreaterThan(0).WithMessage("Pages must be greater than zero.");
}
}