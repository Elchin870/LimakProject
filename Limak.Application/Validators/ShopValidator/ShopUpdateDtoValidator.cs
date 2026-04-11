using FluentValidation;
using Limak.Application.Dtos.ShopDtos;
using Limak.Application.Helpers;

namespace Limak.Application.Validators.ShopValidator;

public class ShopUpdateDtoValidator : AbstractValidator<ShopUpdateDto>
{
    public ShopUpdateDtoValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Shop ID is required.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Shop name is required.").MaximumLength(128).WithMessage("Shop name cannot exceed 128 characters.");
        RuleFor(x => x.WebsitePath).NotEmpty().WithMessage("Website path is required.").MaximumLength(512).WithMessage("Website path cannot exceed 512 characters.");
        RuleFor(x => x.Image).Must(x => x?.CheckSize(2) ?? true).WithMessage("Seklin maksimum olcusu 2 mb olmalidir")
                            .Must(x => x?.CheckType("image") ?? true).WithMessage("Yalniz sekil formatinda data gondere bilersiniz");
    }
}
