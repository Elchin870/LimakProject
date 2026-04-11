using FluentValidation;
using Limak.Application.Dtos.CountryAddressDtos;

namespace Limak.Application.Validators.CountryAddressValidators;

public class CountryAddressCreateDtoValidator : AbstractValidator<CountryAddressCreateDto>
{
    public CountryAddressCreateDtoValidator()
    {
        RuleFor(x => x.City).NotEmpty().WithMessage("City is required").MaximumLength(100).WithMessage("City  must not exceed 100 characters");
        RuleFor(x => x.State).NotEmpty().WithMessage("State is required").MaximumLength(100).WithMessage("State  must not exceed 100 characters");
        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required").MaximumLength(256).WithMessage("Address  must not exceed 256 characters");
        RuleFor(x => x.TC).MaximumLength(100).WithMessage("TC  must not exceed 100 characters");
        RuleFor(x => x.CountryId).NotEmpty().WithMessage("CountryId is required");
    }
}


public class CountryAddressUpdateDtoValidator : AbstractValidator<CountryAddressUpdateDto>
{
    public CountryAddressUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.City).NotEmpty().WithMessage("City is required").MaximumLength(100).WithMessage("City  must not exceed 100 characters");
        RuleFor(x => x.State).NotEmpty().WithMessage("State is required").MaximumLength(100).WithMessage("State  must not exceed 100 characters");
        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required").MaximumLength(256).WithMessage("Address  must not exceed 256 characters");
        RuleFor(x => x.TC).MaximumLength(100).WithMessage("TC  must not exceed 100 characters");
        RuleFor(x => x.CountryId).NotEmpty().WithMessage("CountryId is required");
    }
}
