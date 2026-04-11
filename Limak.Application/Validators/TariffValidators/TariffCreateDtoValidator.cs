using FluentValidation;
using Limak.Application.Dtos.TariffDtos;

namespace Limak.Application.Validators.TariffValidators;

public class TariffCreateDtoValidator : AbstractValidator<TariffCreateDto>
{
    public TariffCreateDtoValidator()
    {
        RuleFor(x => x.BasePrice).GreaterThan(0).WithMessage("BasePrice must be greater than 0");

        RuleFor(x => x.BaseWeightLimit).GreaterThan(0).WithMessage("BaseWeightLimit must be greater than 0");

        RuleFor(x => x.ExtraPricePerKg).GreaterThan(0).When(x => x.ExtraPricePerKg.HasValue).WithMessage("ExtraPricePerKg must be greater than 0");

        RuleFor(x => x.MinWeight).GreaterThanOrEqualTo(0).WithMessage("MinWeight must be greater or equal than 0!");
        RuleFor(x => x.MaxWeight).NotEmpty().WithMessage("MaxWeight cannot be empty").GreaterThan(0).WithMessage("MaxWeight must be greater than 0!");
        RuleFor(x => x).Must(x => x.MinWeight < x.MaxWeight).WithMessage("MinWeight must be less than MaxWeight");
        RuleFor(x => x.ShipmentTypeId).NotEmpty().WithMessage("ShipmentTypeId is required");
        RuleFor(x => x.DeliveryTypeId).NotEmpty().WithMessage("DeliveryTypeId is required");
        RuleFor(x => x.CountryId).NotEmpty().WithMessage("CountryId is required");
    }
}
