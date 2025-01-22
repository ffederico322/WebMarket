using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using WebMarket.Domain.Dto.Category;
using WebMarket.Domain.Dto.Product;
using WebMarket.Domain.Interfaces.Services;
using WebMarket.Domain.Result;

namespace WebMarket.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class ProductController(IProductService productService) : ControllerBase
{
    /// <summary>
    /// Создание продукта
    /// </summary>
    /// <param name="productId"></param>
    /// <remarks>
    /// Request for create product
    /// POST
    /// </remarks>
    [HttpGet("product/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<ProductDto>>> GetProduct(long productId)
    {
        var response = await productService.GetProductByIdAsync(productId);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CollectionResult<ProductDto>>> GetAllProducts()
    {
        var response = await productService.GetAllProductsAsync();
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpGet("category/{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CollectionResult<ProductDto>>> GetProductsByCategory(long categoryId)
    {
        var response = await productService.GetProductsByCategoryAsync(categoryId);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpDelete("products/{productId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<ProductDto>>> DeleteProduct(long productId)
    {
        var response = await productService.DeleteProductAsync(productId);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<ProductDto>>> CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        var response = await productService.AddProductAsync(createProductDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<ProductDto>>> UpdateProduct([FromBody] UpdateProductDto updateProductDto, 
        long productId)
    {
        var response = await productService.UpdateProductAsync(productId, updateProductDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}