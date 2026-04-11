using FluentValidation;
using Limak.Application.Dtos.DeliveryTypeDtos;

namespace Limak.Application.Validators.DeliveryTypeValidators;

public class DeliveryTypeUpdateDtoValidator : AbstractValidator<DeliveryTypeUpdateDto>
{
    public DeliveryTypeUpdateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Delivery name is required.")
            .MaximumLength(100).WithMessage("Delivery name must not exceed 100 characters.");
    }
}
