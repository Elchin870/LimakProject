using FluentValidation;
using Limak.Application.Dtos.AnnouncementDtos;
using Limak.Application.Helpers;

namespace Limak.Application.Validators.AnnouncementValidators;

public class AnnouncementCreateDtoValidator : AbstractValidator<AnnouncementCreateDto>
{
    public AnnouncementCreateDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(128).WithMessage("Title cannot exceed 128 characters.");

        RuleFor(x => x.Description).
            NotEmpty().WithMessage("Description is required.").
            MaximumLength(1024).WithMessage("Description cannot exceed 1024 characters.");

        RuleFor(x => x.Image).Must(x => x?.CheckSize(2) ?? false).WithMessage("Seklin maksimum olcusu 2 mb olmalidir")
                           .Must(x => x?.CheckType("image") ?? false).WithMessage("Yalniz sekil formatinda data gondere bilersiniz");
    }
}
