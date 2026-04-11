using FluentValidation;
using Limak.Application.Dtos.CategoryDtos;

namespace Limak.Application.Validators.CategoryValidator;

public class CategoryUpdateDtoValidator : AbstractValidator<CategoryUpdateDto>
{
    public CategoryUpdateDtoValidator()
    {
        RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(128);
    }
}
