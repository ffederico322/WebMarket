using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using WebMarket.Application.Resources;
using WebMarket.Domain.Dto.Product;
using WebMarket.Domain.Entity;
using WebMarket.Domain.Enum;
using WebMarket.Domain.Extensions;
using WebMarket.Domain.Interfaces.Repositories;
using WebMarket.Domain.Interfaces.Services;
using WebMarket.Domain.Interfaces.Validations;
using WebMarket.Domain.Result;

namespace WebMarket.Application.Services;

public class ProductService(
    IBaseRepository<Product> productRepository,
    IProductValidator productValidator,
    IBaseRepository<Category> categoryRepository,
    ICollectionValidator collectionValidator,
    IBaseValidator<Product> baseValidator,
    IDistributedCache distributedCache,
    ILogger<ProductService> logger,
    IMapper mapper)
    : IProductService
{
    /// <inheritdoc />
    public async Task<CollectionResult<ProductDto>> GetProductsByCategoryAsync(long categoryId)
    {
        ProductDto[] products; 
        try
        {
            products = await productRepository.GetAll()
                .Where(p => p.CategoryId == categoryId)
                .Select(x => new ProductDto(x.Id, x.Name, x.CategoryId, x.Description, x.Image, 
                    x.Price))
                .ToArrayAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new CollectionResult<ProductDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError,
            };
        }

        if (!products.Any())
        {
            logger.LogWarning(ErrorMessage.ProductsNotFound, products.Length);
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

    /// <inheritdoc />
    public async Task<BaseResult> AddProductAsync(CreateProductDto? createProductDto)
    {
        try
        {
            if (createProductDto == null)
            {
                logger.LogWarning("CreateProductDto пустое");
                return new BaseResult<ProductDto>
                {
                    ErrorMessage = ErrorMessage.InvalidInput,
                    ErrorCode = (int)ErrorCodes.InvalidInput
                };
            }
            
            var product = new Product
            {
                Name = createProductDto.Name,
                CategoryId = createProductDto.CategoryId,
                Description = createProductDto.Description,
                Image = createProductDto.Image,
                Price = createProductDto.Price,
                Stock = createProductDto.Stock,
            };
            
            var category = await categoryRepository.GetByIdAsync(product.CategoryId);
            var validationResult = await productValidator.CreateValidator(product, category);
            
            if (!validationResult.IsSuccess)
            {
                logger.LogError(validationResult.ErrorMessage, validationResult.ErrorCode);
                return validationResult;
            }
            
            await productRepository.CreateAsync(product);
            return new BaseResult();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new BaseResult()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError,
            };
        }
    }

    public async Task<BaseResult<ProductDto>> GetProductByIdAsync(long productId)
    { 
        try
        {
            var product = await productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                logger.LogWarning("Продукт с {Id} не найден", productId);
                return new BaseResult<ProductDto>()
                {
                    ErrorMessage = ErrorMessage.ProductNotFound,
                    ErrorCode = (int)ErrorCodes.ProductNotFound,
                };
            }
            
            return new BaseResult<ProductDto>()
            {
                Data = mapper.Map<ProductDto>(product)
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new BaseResult<ProductDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError,
            };
        }
    }

    public async Task<CollectionResult<ProductDto>> GetAllProductsAsync()
    {
        try
        {
            var products = await productRepository.GetAll()
                .Where(p => p.Stock > 0)
                .ToArrayAsync();
            
            if (!products.Any())
            {
                logger.LogWarning(ErrorMessage.ProductsNotFound, products.Length);
                return new CollectionResult<ProductDto>()
                {
                    ErrorMessage = ErrorMessage.ProductsNotFound,
                    ErrorCode = (int)ErrorCodes.ProductsNotFound,
                };
            }
            
            return new CollectionResult<ProductDto>()
            {
                Data = mapper.Map<ProductDto[]>(products),
                Count = products.Length
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new CollectionResult<ProductDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError,
            };
        }
    }
    
    public async Task<BaseResult<ProductDto>> DeleteProductAsync(long productId)
    {
        try
        {
            var product = await productRepository.GetByIdAsync(productId);
            var result = baseValidator.ValidateOnNull(product);
            if (!result.IsSuccess)
            {
                if (!result.IsSuccess)
                {
                    return new BaseResult<ProductDto>()
                    {
                        ErrorMessage = result.ErrorMessage,
                        ErrorCode = result.ErrorCode,
                    };
                }
            }
            
            productRepository.Remove(product);
            await productRepository.SaveChangesAsync();
                
            return new BaseResult<ProductDto>()
            {
                Data = mapper.Map<Product, ProductDto>(product),
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new BaseResult<ProductDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError,
            };
        }
    }

    public async Task<BaseResult<ProductDto>> UpdateProductAsync(long productId ,UpdateProductDto updateProductDto)
    {
        try
        {
            var product = await productRepository.GetByIdAsync(productId);
            var result = baseValidator.ValidateOnNull(product);
            if (!result.IsSuccess)
            {
                if (!result.IsSuccess)
                {
                    return new BaseResult<ProductDto>()
                    {
                        ErrorMessage = result.ErrorMessage,
                        ErrorCode = result.ErrorCode,
                    };
                }
            }
            
            product.Name = updateProductDto.Name;
            product.CategoryId = updateProductDto.CategoryId;
            product.Description = updateProductDto.Description;
            product.Image = updateProductDto.Image;
            product.Price = updateProductDto.Price;
            product.Stock = updateProductDto.Stock;
            
            var updatedProduct = productRepository.Update(product);
            await productRepository.SaveChangesAsync();

            return new BaseResult<ProductDto>()
            {
                Data = mapper.Map<Product, ProductDto>(updatedProduct),
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new BaseResult<ProductDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError,
            };
        }
    }
}