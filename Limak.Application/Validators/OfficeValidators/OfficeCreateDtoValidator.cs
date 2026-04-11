using FluentValidation;
using Limak.Application.Dtos.OfficeDtos;

namespace Limak.Application.Validators.OfficeValidators;

public class OfficeCreateDtoValidator : AbstractValidator<OfficeCreateDto>
{
    public OfficeCreateDtoValidator()
    {
        RuleFor(x => x.City).NotNull().NotEmpty().MaximumLength(100);
        RuleFor(x => x.MetroStation).MaximumLength(100);
    }
}
