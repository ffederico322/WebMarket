using FluentValidation;
using WebMarket.Domain.Dto.CartItem;
using WebMarket.Domain.Entity;

namespace WebMarket.Application.Validations.FluentValidations.CartItemValidation;

public class CartItemValidator : AbstractValidator<CartItemDto>
{
    public CartItemValidator()
    {
        RuleFor(item => item.Id)
            .GreaterThan(0)
            .WithMessage("Id позиции корзины должен быть больше 0");

        RuleFor(item => item.CartId)
            .GreaterThan(0)
            .WithMessage("Id корзины должен быть больше 0");

        RuleFor(item => item.ProductId)
            .GreaterThan(0)
            .WithMessage("Id продукта должен быть больше 0");

        RuleFor(item => item.Quantity)
            .GreaterThan(0)
            .WithMessage("Количество товара должно быть больше 0")
            .LessThanOrEqualTo(100)
            .WithMessage("Количество одного товара не может превышать 100 штук");

        RuleFor(item => item.Price)
            .GreaterThan(0)
            .WithMessage("Цена товара должна быть больше 0")
            .PrecisionScale(10, 2, false)
            .WithMessage("Цена должна быть указана с точностью до 2 знаков после запятой");
    }
}