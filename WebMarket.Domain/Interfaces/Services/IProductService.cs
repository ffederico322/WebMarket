using WebMarket.Domain.Dto;
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
    /// <param name="productCreateDto"></param>
    /// <returns></returns>
    Task<BaseResult> AddProductAsync(ProductCreateDto productCreateDto);
}