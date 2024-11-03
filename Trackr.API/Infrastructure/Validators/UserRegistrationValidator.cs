using FluentValidation;
using Trackr.Application.Models;

namespace Trackr.API.Infrastructure.Validators;

public class UserRegistrationValidator : AbstractValidator<UserRequestModel>
{
    public UserRegistrationValidator()
    {
        RuleFor(user => user.Name).NotEmpty().MinimumLength(2).MaximumLength(50);
        RuleFor(user => user.Email).NotEmpty().EmailAddress().MinimumLength(5).MaximumLength(50);
        RuleFor(user => user.Password).NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
        RuleFor(user => user.ConfirmPassword).Equal(user => user.Password).WithMessage("Passwords do not match");
    }
}
