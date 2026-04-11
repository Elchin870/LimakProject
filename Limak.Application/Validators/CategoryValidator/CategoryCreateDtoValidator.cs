using FluentValidation;
using Limak.Application.Dtos.CategoryDtos;

namespace Limak.Application.Validators.CategoryValidator;

public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
{
    public CategoryCreateDtoValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(128);
    }
}
