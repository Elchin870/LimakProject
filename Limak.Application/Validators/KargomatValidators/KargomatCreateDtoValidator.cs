using FluentValidation;
using Limak.Application.Dtos.KargomatDtos;

namespace Limak.Application.Validators.KargomatValidators;

public class KargomatCreateDtoValidator : AbstractValidator<KargomatCreateDto>
{
    public KargomatCreateDtoValidator()
    {
        RuleFor(x => x.ShortAddress).NotEmpty().WithMessage("Short address is required").MaximumLength(256).WithMessage("Short address must be less than 256 characters");
        RuleFor(x => x.FullAddress).NotEmpty().WithMessage("Full address is required").MaximumLength(512).WithMessage("Full address must be less than 512 characters");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0!");
    }
}

