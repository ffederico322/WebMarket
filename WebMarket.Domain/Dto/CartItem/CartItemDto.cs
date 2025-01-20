namespace WebMarket.Domain.Dto.CartItem;

public record CartItemDto(long Id, long CartId, long ProductId, 
    long Quantity, decimal Price);