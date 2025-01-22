using Microsoft.AspNetCore.Mvc;
using WebMarket.Domain.Dto.Cart;
using WebMarket.Domain.Dto.Category;
using WebMarket.Domain.Dto.Product;
using WebMarket.Domain.Interfaces;
using WebMarket.Domain.Interfaces.Services;
using WebMarket.Domain.Result;

namespace WebMarket.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CartController(ICartService cartService) : ControllerBase
{
    [HttpGet("cart/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<CartDto>>> GetCart(long userId)
    {
        var response = await cartService.GetCartByUserIdAsync(userId);
        if (response.IsSuccess)
        { 
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<CartDto>>> AddToCart(long userId, long productId, int quantity)
    {
        var response = await cartService.AddToCartAsync(userId, productId, quantity);
        if (response.IsSuccess)
        { 
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpDelete("cart/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<CartDto>>> RemoveFromCart(long userId, long productId)
    {
        var response = await cartService.RemoveFromCartAsync(userId, productId);
        if (response.IsSuccess)
        { 
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpPut("cart/{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<CartDto>>> UpdateCartItemQuantity(long userId, long productId, int quantity)
    {
        var response = await cartService.UpdateCartItemQuantityAsync(userId, productId, quantity);
        if (response.IsSuccess)
        { 
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<CartDto>>> ClearCart(long userId)
    {
        var response = await cartService.ClearCartAsync(userId);
        if (response.IsSuccess)
        { 
            return Ok(response);
        }
        return BadRequest(response);
    }
}

