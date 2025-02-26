using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using WebMarket.Application.Services;
using WebMarket.Domain.Entity;
using WebMarket.Tests.Configurations;
using Xunit;

namespace WebMarket.Tests;

public class ProductServiceTest
{
    [Fact]
    public async Task GetProductById_ShouldReturnProductDto()
    {
        // Arrange
        var mockRepository = MockRepositoriesGetter.GetMockProductRepository();
        var mockDistributeCache = new Mock<IDistributedCache>();
        var productService = new ProductService(mockRepository.Object, null, null, 
            null, null, mockDistributeCache.Object, NullLogger<ProductService>.Instance, null);
        
        // Act
        var result = await productService.GetProductByIdAsync(1);
        
        //
        Assert.NotNull(result);
    }
    
    
    
    
}


/*
 *     Task<CollectionResult<ProductDto>> GetProductsByCategoryAsync(long categoryId);

    /// <summary>
    /// Добавление нового продукта в систему
    /// </summary>
    /// <param name="createProductDto"></param>
    /// <returns></returns>
    Task<BaseResult> AddProductAsync(CreateProductDto createProductDto);
    
    Task<BaseResult<ProductDto>> GetProductByIdAsync(long productId);
    
    Task<CollectionResult<ProductDto>> GetAllProductsAsync();
    
    Task<BaseResult<ProductDto>> DeleteProductAsync(long productId);
    
    Task<BaseResult<ProductDto>> UpdateProductAsync(long id, UpdateProductDto updateProductDto);
 */


