using FluentValidation;

namespace BookLibrarySystem.Application.Genres.DeleteGenre;

public class DeleteGenreCommandValidator : AbstractValidator<DeleteGenreCommand>
{
    public DeleteGenreCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Genre ID is required.")
            .NotEqual(Guid.Empty).WithMessage("Genre ID must not be empty.");
    }
}