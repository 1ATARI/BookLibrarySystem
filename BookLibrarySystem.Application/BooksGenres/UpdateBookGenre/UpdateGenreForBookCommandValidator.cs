using FluentValidation;

namespace BookLibrarySystem.Application.BooksGenres.UpdateBookGenre;

public class UpdateGenreForBookCommandValidator : AbstractValidator<UpdateGenreForBookCommand>
{
        public UpdateGenreForBookCommandValidator()
    {
        RuleFor(x => x.BookId)
            .NotEmpty().WithMessage("Book ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Book ID must not be empty.");

        RuleFor(x => x.OldGenreId)
            .NotEmpty().WithMessage("Old Genre ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Old Genre ID must not be empty.");

        RuleFor(x => x.NewGenreId)
            .NotEmpty().WithMessage("New Genre ID is required.")
            .NotEqual(Guid.Empty).WithMessage("New Genre ID must not be empty.")
            .NotEqual(x => x.OldGenreId).WithMessage("New Genre ID must be different from Old Genre ID.");
    }
}