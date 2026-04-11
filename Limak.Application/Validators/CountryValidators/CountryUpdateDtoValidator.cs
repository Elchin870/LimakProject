using FluentValidation;
using Limak.Application.Dtos.CountryDtos;

namespace Limak.Application.Validators.CountryValidators;

public class CountryUpdateDtoValidator : AbstractValidator<CountryUpdateDto>
{
    public CountryUpdateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Country name is required.")
            .MaximumLength(100).WithMessage("Country name must not exceed 100 characters.");

        RuleFor(x => x.VolumetricDivisor).GreaterThan(0).WithMessage("Volumetric divisor must be greater than zero.");
    }
}
