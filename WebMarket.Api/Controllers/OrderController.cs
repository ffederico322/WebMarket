using Microsoft.AspNetCore.Mvc;
using WebMarket.Domain.Dto.Cart;
using WebMarket.Domain.Dto.Order;
using WebMarket.Domain.Interfaces;
using WebMarket.Domain.Result;

namespace WebMarket.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<OrderDto>>> CreateOrder(long userId)
    {
        var response = await orderService.CreateOrderFromCartAsync(userId);
        if (response.IsSuccess)
        { 
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpGet("orders/user/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CollectionResult<OrderDto>>> GetUserOrders(long userId)
    {
        var response = await orderService.GetUserOrdersAsync(userId);
        if (response.IsSuccess)
        { 
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpGet("orders/{orderId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<OrderDto>>> GetOrder(long orderId)
    {
        var response = await orderService.GetOrderByIdAsync(orderId);
        if (response.IsSuccess)
        { 
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<OrderDto>>> CancelOrder(long orderId)
    {
        var response = await orderService.CancelOrderAsync(orderId);
        if (response.IsSuccess)
        { 
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpGet("orders/{userId}/activeorders")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CollectionResult<OrderDto>>> GetActiveOrders(long userId)
    {
        var response = await orderService.GetActiveOrdersAsync(userId);
        if (response.IsSuccess)
        { 
            return Ok(response);
        }
        return BadRequest(response);
    }
    
}