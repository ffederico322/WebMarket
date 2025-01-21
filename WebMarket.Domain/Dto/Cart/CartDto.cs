using WebMarket.Domain.Dto.CartItem;

namespace WebMarket.Domain.Dto.Cart;

public record CartDto(int Id, decimal TotalPrice, IEnumerable<CartItemDto> CartItems, long UserId);