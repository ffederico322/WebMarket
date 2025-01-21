using FluentValidation;
using WebMarket.Application.Validations.FluentValidations.CartItemValidation;
using WebMarket.Domain.Dto.Cart;
using WebMarket.Domain.Entity;

namespace WebMarket.Application.Validations.FluentValidations.CartValidation;

public class CartValidator : AbstractValidator<CartDto>
{
    public CartValidator()
    {
        RuleFor(cart => cart.Id)
            .GreaterThan(0)
            .WithMessage("Id корзины должен быть больше 0");

        RuleFor(cart => cart.TotalPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Общая стоимость корзины не может быть отрицательной");

        RuleFor(cart => cart.CartItems)
            .NotNull()
            .WithMessage("Список товаров в корзине не может быть null")
            .Must(items => items == null || items.Count() <= 100)
            .WithMessage("В корзине не может быть больше 100 различных товаров");

        RuleFor(cart => cart.UserId)
            .GreaterThan(0)
            .WithMessage("Id пользователя должен быть больше 0");

        RuleForEach(cart => cart.CartItems)
            .SetValidator(new CartItemValidator());
    }
}