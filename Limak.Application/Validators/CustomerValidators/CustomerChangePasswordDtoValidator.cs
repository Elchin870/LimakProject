using FluentValidation;
using Limak.Application.Dtos.CustomerDtos;

namespace Limak.Application.Validators.CustomerValidators;

public class CustomerChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
{
    public CustomerChangePasswordDtoValidator()
    {
        RuleFor(x => x.CurrentPassword).NotEmpty().WithMessage("Current Password is required.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New Password is required.")
            .MinimumLength(6).WithMessage("New Password must be at least 6 characters long.")
            .Matches("[A-Z]").WithMessage("New Password must contain at least one uppercase letter.")
            .Matches("[a-z]").WithMessage("New Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("New Password must contain at least one digit.")
            .Matches("[^a-zA-Z0-9]").WithMessage("New Password must contain at least one non-alphanumeric character.");

        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty().WithMessage("Confirm password is required.")
            .Equal(x => x.NewPassword).WithMessage("Confirm password must match the password.");
    }
}
