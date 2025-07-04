using FluentValidation;

namespace BookLibrarySystem.Application.Books.UpdateBook;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
        RuleFor(x => x.BookId)
            .NotEmpty()
            .WithMessage("Book ID is required.");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(100)
            .WithMessage("Title must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.PublicationDate)
            .NotEmpty()
            .WithMessage("Publication date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Publication date cannot be in the future.");

        RuleFor(x => x.Pages)
            .GreaterThan(0)
            .WithMessage("Pages must be greater than zero.");

        RuleFor(x => x.AuthorId)
            .NotEmpty()
            .WithMessage("Author ID is required.");

        RuleFor(x => x.GenreIds)
            .NotNull()
            .WithMessage("Genre IDs cannot be null.")
            .Must(x => x.Any())
            .WithMessage("At least one genre must be specified.")
            .ForEach(id => id.NotEmpty()
                .WithMessage("Genre ID cannot be empty."));
    }
}