using WebMarket.Domain.Dto.Product;

namespace WebMarket.Domain.Dto.OrderItem;

public record OrderItemDto(long Id, int Quantity, decimal Price,
    long ProductId, long OrderId, ProductDto Product);