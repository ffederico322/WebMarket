using FluentValidation;
using WebMarket.Application.Validations.FluentValidations.OrderItemValidation;
using WebMarket.Domain.Dto.Order;

namespace WebMarket.Application.Validations.FluentValidations.OrderValidation;

public class OrderValidator : AbstractValidator<OrderDto>
{
    public OrderValidator()
    {
        RuleFor(order => order.Id)
            .GreaterThan(0)
            .WithMessage("Id заказа должен быть больше 0");

        RuleFor(order => order.TotalPrice)
            .GreaterThan(0)
            .WithMessage("Общая стоимость заказа должна быть больше 0")
            .PrecisionScale(10, 2, false)
            .WithMessage("Общая стоимость должна быть указана с точностью до 2 знаков после запятой");

        RuleFor(order => order.UserId)
            .GreaterThan(0)
            .WithMessage("Id пользователя должен быть больше 0");

        RuleFor(order => order.CartId)
            .GreaterThan(0)
            .WithMessage("Id корзины должен быть больше 0");

        RuleFor(order => order.OrderItems)
            .NotNull()
            .WithMessage("Список товаров в заказе не может быть null")
            .Must(items => items != null && items.Any())
            .WithMessage("Заказ должен содержать хотя бы один товар");

        RuleForEach(order => order.OrderItems)
            .SetValidator(new OrderItemValidator());

        RuleFor(order => order.CreatedAt)
            .NotNull()
            .WithMessage("Дата создания заказа не может быть null")
            .LessThanOrEqualTo(DateTime.UtcNow.ToString())
            .WithMessage("Дата создания заказа не может быть в будущем");

        RuleFor(order => order.UpdatedAt)
            .NotNull()
            .WithMessage("Дата обновления заказа не может быть null")
            .GreaterThanOrEqualTo(order => order.CreatedAt)
            .WithMessage("Дата обновления не может быть раньше даты создания");
    }
}