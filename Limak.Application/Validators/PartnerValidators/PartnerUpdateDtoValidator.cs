using FluentValidation;
using Limak.Application.Dtos.PartnerDtos;
using Limak.Application.Helpers;

namespace Limak.Application.Validators.PartnerValidators;

public class PartnerUpdateDtoValidator : AbstractValidator<PartnerUpdateDto>
{
    public PartnerUpdateDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");

        RuleFor(x => x.Image).Must(x => x?.CheckSize(2) ?? true).WithMessage("Seklin maksimum olcusu 2 mb olmalidir")
                            .Must(x => x?.CheckType("image") ?? true).WithMessage("Yalniz sekil formatinda data gondere bilersiniz");

        RuleFor(x => x.WebsitePath).NotEmpty().WithMessage("Website path is required.").MaximumLength(512).WithMessage("Website path cannot exceed 512 characters.");
    }
}
