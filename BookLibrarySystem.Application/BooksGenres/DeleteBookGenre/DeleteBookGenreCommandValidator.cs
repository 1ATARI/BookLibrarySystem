using FluentValidation;

namespace BookLibrarySystem.Application.BooksGenres.DeleteBookGenre;

public class DeleteBookGenreCommandValidator : AbstractValidator<DeleteBookGenreCommand>
{
    public DeleteBookGenreCommandValidator()
    {
        RuleFor(x => x.BookGenreId)
            .NotEmpty().WithMessage("BookGenre ID is required.")
            .NotEqual(Guid.Empty).WithMessage("BookGenre ID must not be empty.");
    }
}