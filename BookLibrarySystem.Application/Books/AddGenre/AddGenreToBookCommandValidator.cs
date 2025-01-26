using FluentValidation;

namespace BookLibrarySystem.Application.Books.AddGenre;


public class AddGenreToBookCommandValidator : AbstractValidator<AddGenreToBookCommand>
{
    public AddGenreToBookCommandValidator()
    {
        RuleFor(x => x.BookId)
            .NotEmpty().WithMessage("Book ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Book ID must not be empty.");

        RuleFor(x => x.GenreId)
            .NotEmpty().WithMessage("Genre ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Genre ID must not be empty.");
    }
}