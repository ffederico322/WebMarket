using WebMarket.Application.Resources;
using WebMarket.Domain.Dto.Product;
using WebMarket.Domain.Entity;
using WebMarket.Domain.Enum;
using WebMarket.Domain.Interfaces.Repositories;
using WebMarket.Domain.Interfaces.Validations;
using WebMarket.Domain.Result;

namespace WebMarket.Application.Validations.ProductValidators;

public class ProductValidator(ICategoryRepository categoryRepository) : IProductValidator
{
    public async Task<BaseResult> CreateValidator(Product product, Category category)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            return new BaseResult
            {
                ErrorMessage = ErrorMessage.InvalidProductName,
                ErrorCode = (int)ErrorCodes.InvalidProductName
            };
        }

        if (!await categoryRepository.ExistsAsync((int)product.CategoryId))
        {
            return new BaseResult
            {
                ErrorMessage = ErrorMessage.CategoryNotFound,
                ErrorCode = (int)ErrorCodes.CategoryNotFound
            };
        }

        if (string.IsNullOrWhiteSpace(product.Description))
        {
            return new BaseResult
            {
                ErrorMessage = ErrorMessage.InvalidProductDescription,
                ErrorCode = (int)ErrorCodes.InvalidProductDescription
            };
        }
        
        if (product.Price <= 0)
        {
            return new BaseResult
            {
                ErrorMessage = ErrorMessage.InvalidProductPrice,
                ErrorCode = (int)ErrorCodes.InvalidProductPrice
            };
        }

        if (product.Stock < 0)
        {
            return new BaseResult
            {
                ErrorMessage = ErrorMessage.InvalidProductStock,
                ErrorCode = (int)ErrorCodes.InvalidProductStock
            };
        }
        
        return new BaseResult();
    }
}