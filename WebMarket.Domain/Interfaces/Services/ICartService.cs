using WebMarket.Domain.Dto.Cart;
using WebMarket.Domain.Result;

namespace WebMarket.Domain.Interfaces;

public interface ICartService
{
    Task<BaseResult<CartDto>> GetCartByUserIdAsync(long userId);
    
    Task<BaseResult<CartDto>> AddToCartAsync(long userId, long productId, int quantity);

    Task<BaseResult<CartDto>> RemoveFromCartAsync(long userId, long productId);
    
    Task<BaseResult<CartDto>> UpdateCartItemQuantityAsync(long userId, long productId, int quantity);
    
    Task<BaseResult<CartDto>> ClearCartAsync(long userId);
}