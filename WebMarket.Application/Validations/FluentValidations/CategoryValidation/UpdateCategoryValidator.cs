using FluentValidation;
using WebMarket.Domain.Dto.Category;

namespace WebMarket.Application.Validations.FluentValidations.CategoryValidation;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryValidator()
    {
        // Правило для поля Name
        RuleFor(x => x.Name)
            .NotEmpty() // Обязательное поле
            .MaximumLength(255);  // Максимальная длина
        
        // Если, например, вам нужно запретить создание категории без названия:
        RuleFor(x => x)
            .Must(category => !string.IsNullOrWhiteSpace(category.Name));
    }
}