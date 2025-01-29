using FluentValidation;

namespace BookLibrarySystem.Application.Users.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.UserDto.FirstName)
            .MaximumLength(100).WithMessage("First Name cannot be longer than 100 characters.")
            .OverridePropertyName("FirstName");

        RuleFor(x => x.UserDto.LastName)
            .MaximumLength(100).WithMessage("Last Name cannot be longer than 100 characters.")
            .OverridePropertyName("LastName");

        RuleFor(x => x.UserDto.Email)
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(150).WithMessage("Email cannot be longer than 150 characters.")
            .OverridePropertyName("Email");

        RuleFor(x => x.UserDto.Username)
            .MaximumLength(100).WithMessage("Username cannot be longer than 100 characters.")
            .OverridePropertyName("Username");

        RuleFor(x => x.UserDto.PhoneNumber)
            .Matches(@"^\+?\d{10,15}$").WithMessage("Phone number is not valid.")
            .When(x => !string.IsNullOrWhiteSpace(x.UserDto.PhoneNumber))
            .OverridePropertyName("PhoneNumber");

        RuleFor(x => x.UserDto.Password)
            .MinimumLength(6).WithMessage("Password should be at least 6 characters.")
            .When(x => !string.IsNullOrWhiteSpace(x.UserDto.Password))
            .OverridePropertyName("Password");
    }
}