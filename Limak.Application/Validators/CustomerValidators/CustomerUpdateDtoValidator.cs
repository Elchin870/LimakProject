using FluentValidation;
using Limak.Application.Dtos.CustomerDtos;

namespace Limak.Application.Validators.CustomerValidators;

public class CustomerUpdateDtoValidator : AbstractValidator<CustomerUpdateDto>
{
    public CustomerUpdateDtoValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required").MaximumLength(256).WithMessage("Firstname must not exceed 256 characters");
        RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName is required").MaximumLength(256).WithMessage("LastName must not exceed 256 characters");
        RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required").MinimumLength(3).WithMessage("Username must be at least 3 characters long.").MaximumLength(50).WithMessage("Username must not exceed 50 characters.");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Email must be a valid email address");
        RuleFor(x => x.PhoneNumber).NotEmpty().WithMessage("Phone number is required").Matches(@"^\+?[0-9]{7,15}$").WithMessage("Phone number must be a valid phone number");
        RuleFor(x => x.OfficeId).NotEmpty().WithMessage("Office Id is required");
    }
}
