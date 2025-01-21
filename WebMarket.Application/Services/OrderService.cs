using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebMarket.Application.Resources;
using WebMarket.Domain.Dto.Cart;
using WebMarket.Domain.Dto.Order;
using WebMarket.Domain.Entity;
using WebMarket.Domain.Enum;
using WebMarket.Domain.Interfaces;
using WebMarket.Domain.Interfaces.Repositories;
using WebMarket.Domain.Interfaces.Validations;
using WebMarket.Domain.Result;

namespace WebMarket.Application.Services;

public class OrderService(
    IBaseRepository<Order> orderRepository,
    IBaseRepository<OrderItem> orderItemRepository,
    IBaseRepository<Product> productRepository,
    IBaseRepository<Cart> cartRepository,
    IBaseValidator<Order> baseValidator,
    ILogger<OrderService> logger,
    IMapper mapper) : IOrderService
{
    public async Task<BaseResult<OrderDto>> CreateOrderFromCartAsync(long userId)
    {
        try
        {
            var cart = await cartRepository.GetAll()
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return new BaseResult<OrderDto>
                {
                    ErrorMessage = ErrorMessage.EmptyCart,
                    ErrorCode = (int)ErrorCodes.EmptyCart
                };
            }

            var order = new Order
            {
                UserId = userId,
                CartId = cart.Id,
                IsActive = true,
                TotalPrice = cart.TotalPrice,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    Price = ci.Price,
                }).ToList()
            };
            
            await orderRepository.CreateAsync(order);

            return new BaseResult<OrderDto>
            {
                Data = mapper.Map<OrderDto>(order)
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new BaseResult<OrderDto>
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }
    }

    public async Task<CollectionResult<OrderDto>> GetUserOrdersAsync(long userId)
    {
        try
        {
            var orders = await orderRepository.GetAll()
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            if (!orders.Any())
            {
                return new CollectionResult<OrderDto>()
                {
                    ErrorMessage = ErrorMessage.OrdersNotFound,
                    ErrorCode = (int)ErrorCodes.OrdersNotFound
                };
            }

            return new CollectionResult<OrderDto>()
            {
                Data = mapper.Map<List<OrderDto>>(orders),
                Count = orders.Count
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new CollectionResult<OrderDto>
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }
    }

    public async Task<BaseResult<OrderDto>> GetOrderByIdAsync(long orderId)
    {
        try
        {
            var order = await orderRepository.GetByIdAsync(orderId);
            
            var result = baseValidator.ValidateOnNull(order);
            if (!result.IsSuccess)
            {
                return new BaseResult<OrderDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode
                };
            }

            return new BaseResult<OrderDto>()
            {
                Data = mapper.Map<OrderDto>(order)
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new BaseResult<OrderDto>
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }
    }

    public async Task<BaseResult<OrderDto>> CancelOrderAsync(long orderId)
    {
        try
        {
            var order = await orderRepository.GetByIdAsync(orderId);
            
            var result = baseValidator.ValidateOnNull(order);
            if (!result.IsSuccess)
            {
                return new BaseResult<OrderDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode
                };
            }

            if (!order.IsActive)
            {
                return new BaseResult<OrderDto>()
                {
                    ErrorMessage = ErrorMessage.OrderAlreadyCancelled,
                    ErrorCode = (int)ErrorCodes.OrderAlreadyCancelled
                };
            }
            
            order.IsActive = false;
            order.UpdatedAt = DateTime.Now;
            
            await orderRepository.UpdateAsync(order);

            return new BaseResult<OrderDto>
            {
                Data = mapper.Map<OrderDto>(order)
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new BaseResult<OrderDto>
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }
    }

    public async Task<CollectionResult<OrderDto>> GetActiveOrdersAsync(long userId)
    {
        try
        {
            var orders = await orderRepository.GetAll()
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId && !o.IsActive)
                .ToListAsync();

            if (!orders.Any())
            {
                return new CollectionResult<OrderDto>()
                {
                    ErrorMessage = ErrorMessage.OrdersNotFound,
                    ErrorCode = (int)ErrorCodes.OrdersNotFound
                };
            }

            return new CollectionResult<OrderDto>()
            {
                Data = mapper.Map<List<OrderDto>>(orders),
                Count = orders.Count
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new CollectionResult<OrderDto>
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError
            };
        }
    }
}