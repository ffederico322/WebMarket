using FluentValidation;
using WebMarket.Domain.Dto.Product;

namespace WebMarket.Application.Validations.FluentValidations.Report;

public class CreateProductValidator : AbstractValidator<CreateProductDto>
{
    public CreateProductValidator()
    {
        // Проверка для Name
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название продукта обязательно для заполнения.")
            .MaximumLength(100).WithMessage("Название продукта должно быть длиной не более 100 символов.");
            
        // Проверка для CategoryId
        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("ID категории должен быть положительным числом.");
            
        // Проверка для Description
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Описание продукта обязательно для заполнения.")
            .MaximumLength(1000).WithMessage("Описание продукта должно быть длиной не более 1000 символов.");

        // Проверка для Image (формат изображения)
        RuleFor(x => x.Image)
            .NotEmpty().WithMessage("Изображение продукта обязательно для загрузки.")
            .Matches(@"^.*\.(jpg|jpeg|png|gif)$").WithMessage("Изображение должно быть в формате jpg, jpeg, png или gif.");

        // Проверка для Price
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Цена продукта должна быть больше нуля.");

        // Проверка для Stock
        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("Количество товара не может быть отрицательным.");
    }
}