using WebMarket.Domain.Dto.OrderItem;

namespace WebMarket.Domain.Dto.Order;

public record OrderDto(int Id, bool IsActive, decimal TotalPrice, 
    long UserId, long CartId, IEnumerable<OrderItemDto> OrderItems, DateTime CreatedAt, DateTime UpdatedAt);