using WebMarket.Domain.Dto.Order;
using WebMarket.Domain.Result;

namespace WebMarket.Domain.Interfaces;

public interface IOrderService
{
    /// <summary>
    /// Создает новый заказ из корзины пользователя
    /// </summary>
    Task<BaseResult<OrderDto>> CreateOrderFromCartAsync(long userId);

    /// <summary>
    /// Получает все заказы пользователя
    /// </summary>
    Task<CollectionResult<OrderDto>> GetUserOrdersAsync(long userId);

    /// <summary>
    /// Получает заказ по его ID
    /// </summary>
    Task<BaseResult<OrderDto>> GetOrderByIdAsync(long orderId);

    /// <summary>
    /// Отменяет заказ
    /// </summary>
    Task<BaseResult<OrderDto>> CancelOrderAsync(long orderId);

    /// <summary>
    /// Получает все активные заказы пользователя
    /// </summary>
    Task<CollectionResult<OrderDto>> GetActiveOrdersAsync(long userId);
}