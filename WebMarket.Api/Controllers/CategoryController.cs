using Microsoft.AspNetCore.Mvc;
using WebMarket.Domain.Dto.Category;
using WebMarket.Domain.Dto.Product;
using WebMarket.Domain.Interfaces.Services;
using WebMarket.Domain.Result;

namespace WebMarket.Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet("categories/category/{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<CategoryDto>>> GetCategory(long categoryId)
    {
        var response = await categoryService.GetCategoryByIdAsync(categoryId);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CollectionResult<CategoryDto>>> GetAllCategories()
    {
        var response = await categoryService.GetAllCategoriesAsync();
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<CategoryDto>>> CreateProduct([FromBody] CreateCategoryDto createCategoryDto)
    {
        var response = await categoryService.CreateCategoryAsync(createCategoryDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<CategoryDto>>> UpdateCategory([FromBody] CategoryDto categoryDto, 
        long categoryId)
    {
        var response = await categoryService.UpdateCategoryAsync(categoryId, categoryDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
    
    [HttpDelete("categories/{categoryId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<BaseResult<CategoryDto>>> DeleteCategory(long categoryId)
    {
        var response = await categoryService.DeleteCategoryAsync(categoryId);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}