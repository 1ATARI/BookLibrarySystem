using FluentValidation;

namespace BookLibrarySystem.Application.Users.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.CommandDTO.FirstName)
            .NotEmpty().WithMessage("First name is required.")
            .MaximumLength(50).WithMessage("First name must not exceed 50 characters.")
            .OverridePropertyName("First name"); 

        RuleFor(x => x.CommandDTO.LastName)
            .NotEmpty().WithMessage("Last name is required.")
            .MaximumLength(50).WithMessage("Last name must not exceed 50 characters.")
            .OverridePropertyName("Last name"); 

        RuleFor(x => x.CommandDTO.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date of birth cannot be in the future.")
            .OverridePropertyName("Date of birth"); 

        RuleFor(x => x.CommandDTO.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.")
            .OverridePropertyName("Email"); 

        RuleFor(x => x.CommandDTO.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(50).WithMessage("Username must not exceed 50 characters.")
            .OverridePropertyName("Username"); 

        RuleFor(x => x.CommandDTO.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.")
            .OverridePropertyName("Password"); 
    }
}
