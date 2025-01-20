using WebMarket.Domain.Dto;
using WebMarket.Domain.Dto.Category;
using WebMarket.Domain.Dto.Product;
using WebMarket.Domain.Result;

namespace WebMarket.Domain.Interfaces.Services;

/// <summary>
/// Сервис отвечает за работу доменной части продукта (Product)
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Получение всех продуктов по категориям
    /// </summary>
    /// <param name="categoryId"></param>
    /// <returns></returns>
    Task<CollectionResult<ProductDto>> GetProductsByCategoryAsync(long categoryId);

    /// <summary>
    /// Добавление нового продукта в систему
    /// </summary>
    /// <param name="createProductDto"></param>
    /// <param name="categoryDto"></param>
    /// <returns></returns>
    Task<BaseResult> AddProductAsync(CreateProductDto createProductDto, CategoryDto categoryDto);
    
    Task<BaseResult<ProductDto>> GetProductByIdAsync(long productId);
    
    Task<CollectionResult<ProductDto>> GetAllProductsAsync();
    
    Task<BaseResult<ProductDto>> DeleteProductAsync(long productId);
    
    Task<BaseResult<ProductDto>> UpdateProductAsync(long id, UpdateProductDto updateProductDto);
}