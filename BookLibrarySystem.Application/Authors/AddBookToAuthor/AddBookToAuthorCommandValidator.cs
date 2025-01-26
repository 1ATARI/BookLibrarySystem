using FluentValidation;

namespace BookLibrarySystem.Application.Authors.AddBookToAuthor;

public class AddBookToAuthorCommandValidator : AbstractValidator<AddBookToAuthorCommand>
{
    public AddBookToAuthorCommandValidator()
    {
        RuleFor(x => x.AuthorId)
            .NotEmpty().WithMessage("Author ID is required.");

        RuleFor(x => x.BookDto)
            .NotNull().WithMessage("Book information is required.");
        
        RuleFor(x => x.BookDto.Title)
            .NotEmpty().WithMessage("Book title is required.");
        RuleFor(x => x.BookDto.PublicationDate)
            .NotEqual(default(DateTime)).WithMessage("Publication date is required.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Publication date cannot be in the future.");
        RuleFor(x => x.BookDto.Pages)
            .GreaterThan(0).WithMessage("Pages must be greater than zero.");
    }
}