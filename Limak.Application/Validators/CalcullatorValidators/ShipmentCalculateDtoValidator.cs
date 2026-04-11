using FluentValidation;
using Limak.Application.Dtos.CalcullatorDtos;

namespace Limak.Application.Validators.CalcullatorDtos;

public class ShipmentCalculateDtoValidator : AbstractValidator<ShipmentCalculateDto>
{
    private const decimal VolumetricThreshold = 100;

    public ShipmentCalculateDtoValidator()
    {
        RuleFor(x => x.ShipmentTypeId)
            .NotEmpty().WithMessage("ShipmentTypeId is required");

        RuleFor(x => x.DeliveryTypeId)
            .NotEmpty().WithMessage("DeliveryTypeId is required");

        RuleFor(x => x.CountryId)
            .NotEmpty().WithMessage("CountryId is required");

        RuleFor(x => x.Weight)
            .GreaterThan(0)
            .WithMessage("Weight must be greater than 0");

        RuleFor(x => x.Length)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Length.HasValue)
            .WithMessage("Length must be greater than 0");

        RuleFor(x => x.Width)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Width.HasValue)
            .WithMessage("Width must be greater than 0");

        RuleFor(x => x.Height)
            .GreaterThanOrEqualTo(0)
            .When(x => x.Height.HasValue)
            .WithMessage("Height must be greater than 0");

        RuleFor(x => x)
            .Must(HaveAllDimensionsIfVolumetricApplies)
            .WithMessage("All dimensions must be provided when volumetric rule applies");
    }

    private bool HaveAllDimensionsIfVolumetricApplies(ShipmentCalculateDto dto)
    {
        bool thresholdExceeded =
            (dto.Width.HasValue && dto.Width.Value >= VolumetricThreshold) ||
            (dto.Length.HasValue && dto.Length.Value >= VolumetricThreshold) ||
            (dto.Height.HasValue && dto.Height.Value >= VolumetricThreshold);

        if (!thresholdExceeded)
            return true;

        return dto.Width.HasValue &&
               dto.Length.HasValue &&
               dto.Height.HasValue;
    }
}
