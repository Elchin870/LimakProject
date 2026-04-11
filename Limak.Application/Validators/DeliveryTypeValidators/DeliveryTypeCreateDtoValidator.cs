using FluentValidation;
using Limak.Application.Dtos.DeliveryTypeDtos;

namespace Limak.Application.Validators.DeliveryTypeValidators;

public class DeliveryTypeCreateDtoValidator : AbstractValidator<DeliveryTypeCreateDto>
{
    public DeliveryTypeCreateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Delivery name is required.")
            .MaximumLength(100).WithMessage("Delivery name must not exceed 100 characters.");
    }
}
