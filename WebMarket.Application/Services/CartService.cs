using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebMarket.Application.Resources;
using WebMarket.Domain.Dto.Cart;
using WebMarket.Domain.Entity;
using WebMarket.Domain.Enum;
using WebMarket.Domain.Interfaces;
using WebMarket.Domain.Interfaces.Repositories;
using WebMarket.Domain.Interfaces.Validations;
using WebMarket.Domain.Result;

namespace WebMarket.Application.Services;

public class CartService(
    IBaseRepository<Cart> cartRepository,
    IBaseRepository<CartItem> cartItemRepository,
    IBaseRepository<Product> productRepository,
    IBaseValidator<Cart> baseValidator,
    ILogger<CartService> logger,
    IMapper mapper) : ICartService
{
    public async Task<BaseResult<CartDto>> GetCartByUserIdAsync(long userId)
    {
        try
        {
            var cart = await cartRepository.GetAll()
                .Include(x => x.CartItems)
                .FirstOrDefaultAsync(x => x.UserId == userId);
            
            var result = baseValidator.ValidateOnNull(cart);
            if (!result.IsSuccess)
            {
                return new BaseResult<CartDto>()
                {
                    ErrorMessage = ErrorMessage.CartNotFound,
                    ErrorCode = (int)ErrorCodes.CartNotFound
                };
            }

            return new BaseResult<CartDto>()
            {
                Data = mapper.Map<Cart, CartDto>(cart),
            };

        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new BaseResult<CartDto>
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }
    }

    public async Task<BaseResult<CartDto>> AddToCartAsync(long userId, long productId, int quantity)
    {
        try
        {
            var cart = await cartRepository.GetAll()
                .Include(x => x.CartItems)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (cart == null)
            {
                cart = new Cart()
                {
                    UserId = userId,
                    CartItems = new List<CartItem>(),
                    TotalPrice = 0
                };
                await cartRepository.CreateAsync(cart);
            }
            
            var product = await productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return new BaseResult<CartDto>()
                {
                    ErrorMessage = ErrorMessage.ProductNotFound,
                    ErrorCode = (int)ErrorCodes.ProductNotFound
                };
            }
            
            var existingItem = cart.CartItems.FirstOrDefault(x => x.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.Price = product.Price * existingItem.Quantity;
                await cartRepository.UpdateAsync(cart);
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity,
                    Price = product.Price * quantity
                };
                cart.CartItems.Add(cartItem);
                await cartItemRepository.CreateAsync(cartItem);
            }
            
            cart.TotalPrice = cart.CartItems.Sum(x => x.Price);
            await cartRepository.UpdateAsync(cart);

            return new BaseResult<CartDto>()
            {
                Data = mapper.Map<CartDto>(cart),
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new BaseResult<CartDto>
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }
    }

    public async Task<BaseResult<CartDto>> RemoveFromCartAsync(long userId, long productId)
    {
        try
        {
            var cart = await cartRepository.GetAll()
                .Include(x => x.CartItems)
                .FirstOrDefaultAsync(x => x.UserId == userId);
            
            var result = baseValidator.ValidateOnNull(cart);
            if (!result.IsSuccess)
            {
                return new BaseResult<CartDto>()
                {
                    ErrorMessage = ErrorMessage.CartNotFound,
                    ErrorCode = (int)ErrorCodes.CartNotFound
                };
            }
            
            var cartItem = cart.CartItems
                .FirstOrDefault(x => x.ProductId == productId);

            if (cartItem == null)
            {
                return new BaseResult<CartDto>
                {
                    ErrorMessage = ErrorMessage.CartItemNotFound,
                    ErrorCode = (int)ErrorCodes.CartItemNotFound
                };
            }
            
            await cartItemRepository.RemoveAsync(cartItem);
            
            cart.TotalPrice = cart.CartItems.Sum(x => x.Price);
            await cartRepository.UpdateAsync(cart);

            return new BaseResult<CartDto>
            {
                Data = mapper.Map<CartDto>(cart),
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new BaseResult<CartDto>
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }
    }

    public async Task<BaseResult<CartDto>> UpdateCartItemQuantityAsync(long userId, long productId, int quantity)
    {
        try
        {
            var cart = await cartRepository.GetAll()
                .Include(x => x.CartItems)
                .FirstOrDefaultAsync(x => x.UserId == userId);
            
            var result = baseValidator.ValidateOnNull(cart);
            if (!result.IsSuccess)
            {
                return new BaseResult<CartDto>
                {
                    ErrorMessage = ErrorMessage.CartNotFound,
                    ErrorCode = (int)ErrorCodes.CartNotFound
                };
            }
            
            var cartItem = cart.CartItems
                .FirstOrDefault(x => x.ProductId == productId);

            if (cartItem == null)
            {
                return new BaseResult<CartDto>
                {
                    ErrorMessage = ErrorMessage.CartItemNotFound,
                    ErrorCode = (int)ErrorCodes.CartItemNotFound
                };
            }
            
            var product = await productRepository.GetByIdAsync(productId);
            cartItem.Quantity = quantity;
            cartItem.Price = product.Price * quantity;
            
            await cartItemRepository.UpdateAsync(cartItem);
            
            cart.TotalPrice = cart.CartItems.Sum(x => x.Price);
            await cartRepository.UpdateAsync(cart);

            return new BaseResult<CartDto>
            {
                Data = mapper.Map<CartDto>(cart),
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new BaseResult<CartDto>
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }
    }

    public async Task<BaseResult<CartDto>> ClearCartAsync(long userId)
    {
        try
        {
            var cart = cartRepository.GetAll()
                .Include(x => x.CartItems)
                .FirstOrDefault(x => x.UserId == userId);
            
            var result = baseValidator.ValidateOnNull(cart);
            if (!result.IsSuccess)
            {
                return new BaseResult<CartDto>
                {
                    ErrorMessage = ErrorMessage.CartNotFound,
                    ErrorCode = (int)ErrorCodes.CartNotFound
                };
            }
            
            var cartItems = cart.CartItems.ToList();

            foreach (var cartItem in cartItems)
            {
                await cartItemRepository.RemoveAsync(cartItem);
            }
            
            cart.CartItems.Clear();
            cart.TotalPrice = 0;
            await cartRepository.UpdateAsync(cart);

            return new BaseResult<CartDto>
            {
                Data = mapper.Map<CartDto>(cart),
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new BaseResult<CartDto>
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }
    }
}