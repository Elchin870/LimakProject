using FluentValidation;
using Limak.Application.Dtos.OfficeDtos;

namespace Limak.Application.Validators.OfficeValidators;

public class OfficeUpdateDtoValidator : AbstractValidator<OfficeUpdateDto>
{
    public OfficeUpdateDtoValidator()
    {
        RuleFor(x => x.City).NotNull().NotEmpty().MaximumLength(100);
        RuleFor(x => x.MetroStation).MaximumLength(100);
    }
}
