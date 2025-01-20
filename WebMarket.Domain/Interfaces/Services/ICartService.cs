using WebMarket.Domain.Dto.Cart;
using WebMarket.Domain.Dto.CartItem;
using WebMarket.Domain.Result;

namespace WebMarket.Domain.Interfaces.Services;

/// <summary>
/// Сервис отвечает работу доменной части корзины (Cart)
/// </summary>
public interface ICartService
{
    /// <summary>
    /// Получение корзины по ID пользователя 
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<BaseResult<CartDto>> GetCartByUserIdAsync(string userId);
    
    Task<BaseResult<CartDto>> AddToCartAsync(string userId, CartItemDto cartItemDto);
    
    Task<BaseResult<CartDto>> DeleteCartAsync(string userId, string cartId);
    
    Task<BaseResult<CartItemDto>> GetCartItemByIdAsync(string userId, string cartId);
    
    Task<CollectionResult<CartItemDto>> GetCartItemsByIdAsync(string userId, string cartId);
    
    Task<BaseResult<CartItemDto>> UpdateCartItemAsync(string userId, CartItemDto cartItemDto);
    
}

