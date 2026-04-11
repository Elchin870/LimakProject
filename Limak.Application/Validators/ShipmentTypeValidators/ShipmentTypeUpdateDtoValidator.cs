using FluentValidation;
using Limak.Application.Dtos.ShipmentTypeDtos;

namespace Limak.Application.Validators.ShipmentTypeValidators;

public class ShipmentTypeUpdateDtoValidator : AbstractValidator<ShipmentTypeUpdateDto>
{
    public ShipmentTypeUpdateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Shipment name is required.")
            .MaximumLength(100).WithMessage("Shipment name must not exceed 100 characters.");
    }
}
