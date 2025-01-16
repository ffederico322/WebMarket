using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using WebMarket.Application.Resources;
using WebMarket.Domain.Dto;
using WebMarket.Domain.Dto.Product;
using WebMarket.Domain.Entity;
using WebMarket.Domain.Enum;
using WebMarket.Domain.Interfaces.Repositories;
using WebMarket.Domain.Interfaces.Services;
using WebMarket.Domain.Result;

namespace WebMarket.Application.Services;

public class ProductService : IProductService
{
    private readonly IBaseRepository<Product> _productRepository;
    private readonly ILogger _logger;

    public ProductService(IBaseRepository<Product> productRepository, ILogger logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }
    
    /// <inheritdoc />
    public async Task<CollectionResult<ProductDto>> GetProductsByCategoryAsync(long categoryId)
    {
        ProductDto[] products;
        try
        {
            products = await _productRepository.GetAll()
                .Where(p => p.CategoryId == categoryId)
                .Select(x => new ProductDto(x.Id, x.Name, x.CategoryId, x.Description, x.Image, 
                    x.Price))
                .ToArrayAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new CollectionResult<ProductDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError,
            };
        }

        if (!products.Any())
        {
            _logger.LogWarning(ErrorMessage.ProductsNotFound, products.Length);
            return new CollectionResult<ProductDto>()
            {
                ErrorMessage = ErrorMessage.ProductsNotFound,
                ErrorCode = (int)ErrorCodes.ProductsNotFound,
            };
        }

        return new CollectionResult<ProductDto>()
        {
            Data = products,
            Count = products.Length
        };
    }

    public async Task<BaseResult> AddProductAsync(ProductCreateDto productCreateDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(productCreateDto.Name) || productCreateDto.Price <= 0)
            {
                return new BaseResult()
                {
                    ErrorMessage = ErrorMessage.InvalidProductDataError,
                    ErrorCode = (int)ErrorCodes.InvalidProductDataError,
                };
            }
            
            var product = new Product
            {
                Name = productCreateDto.Name,
                CategoryId = productCreateDto.CategoryId,
                Description = productCreateDto.Description,
                Image = productCreateDto.Image,
                Price = productCreateDto.Price,
                Stock = productCreateDto.Stock,
            };
            
            
            await _productRepository.CreateAsync(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return new BaseResult()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError,
            };
        }
        
        return new BaseResult()
        {
            ErrorMessage = null
        };
    }
}