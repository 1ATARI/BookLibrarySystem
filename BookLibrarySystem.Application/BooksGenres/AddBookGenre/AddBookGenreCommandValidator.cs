using FluentValidation;

namespace BookLibrarySystem.Application.BooksGenres.AddBookGenre;

public class AddBookGenreCommandValidator : AbstractValidator<AddBookGenreCommand>
{
    public AddBookGenreCommandValidator()
    {
        RuleFor(x => x.BookId)
            .NotEmpty().WithMessage("Book ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Book ID must not be empty.");

        RuleFor(x => x.GenreId)
            .NotEmpty().WithMessage("Genre ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Genre ID must not be empty.");
    }
}