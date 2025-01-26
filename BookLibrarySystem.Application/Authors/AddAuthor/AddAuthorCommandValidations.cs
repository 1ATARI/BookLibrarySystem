using FluentValidation;

namespace BookLibrarySystem.Application.Authors.AddAuthor;

public class AddAuthorCommandValidations : AbstractValidator<AddAuthorCommand>
{
    public AddAuthorCommandValidations()
    {
        RuleFor(x => x.Name.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

        RuleFor(x => x.Name.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");        
    }
    
}