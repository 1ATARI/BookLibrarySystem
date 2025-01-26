using FluentValidation;

namespace BookLibrarySystem.Application.Books.UpdateBook;

public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
{
    public UpdateBookCommandValidator()
    {
        RuleFor(x => x.BookId)
            .NotEmpty().WithMessage("Book ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Book ID must not be empty.");

        RuleFor(x => x.BookDto.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(x => x.BookDto.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.BookDto.PublicationDate)
            .NotEmpty().WithMessage("Publication date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Publication date cannot be in the future.");

        RuleFor(x => x.BookDto.Pages)
            .GreaterThan(0).WithMessage("Pages must be greater than zero.");

        RuleFor(x => x.BookDto.AuthorId)
            .NotEmpty().WithMessage("Author ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Author ID must not be empty.");
    }
}
