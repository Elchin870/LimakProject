using FluentValidation;
using Limak.Application.Dtos.OrderDtos;

namespace Limak.Application.Validators.OrderValidators;

public class UserOrderCreateDtoValidator : AbstractValidator<UserOrderCreateDto>
{
    public UserOrderCreateDtoValidator()
    {
        RuleFor(x => x.CountryId).NotEmpty().WithMessage("CountryId is required.");
        RuleFor(x => x.StoreName).NotEmpty().WithMessage("StoreName is required.").MaximumLength(128).WithMessage("StoreName must be at most 128 characters long.");
        RuleFor(x => x.StoreUrl).NotEmpty().WithMessage("StoreUrl is required.").MaximumLength(512).WithMessage("StoreUrl must be at most 512 characters long.");
        RuleFor(x => x.PriceAzn).GreaterThan(0).WithMessage("PriceAzn must be greater than 0.");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    }
}
