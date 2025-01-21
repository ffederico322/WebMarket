using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebMarket.Application.Resources;
using WebMarket.Domain.Dto.Category;
using WebMarket.Domain.Dto.Product;
using WebMarket.Domain.Entity;
using WebMarket.Domain.Enum;
using WebMarket.Domain.Interfaces.Repositories;
using WebMarket.Domain.Interfaces.Services;
using WebMarket.Domain.Interfaces.Validations;
using WebMarket.Domain.Result;

namespace WebMarket.Application.Services;

public class CategoryService(
    IBaseRepository<Category> categoryRepository,
    IBaseValidator<Category> baseValidator,
    ILogger logger,
    IMapper mapper) : ICategoryService
{
    public async Task<CollectionResult<CategoryDto>> GetAllCategoriesAsync()
    {
        CategoryDto[] categories;
        try
        {
            categories = await categoryRepository.GetAll()
                .Select(x => new CategoryDto(x.Id, x.Name, x.Products.Count))
                .ToArrayAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new CollectionResult<CategoryDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError,
            };
        }

        if (categories.Any())
        {
            logger.LogWarning(ErrorMessage.CategoriesNotFound, categories.Length);
            return new CollectionResult<CategoryDto>()
            {
                ErrorMessage = ErrorMessage.CategoriesNotFound,
                ErrorCode = (int)ErrorCodes.CategoriesNotFound,
            };
        }
        
        return new CollectionResult<CategoryDto>()
        {
            Data = categories,
            Count = categories.Length
        };
    }

    public async Task<BaseResult<CategoryDto>> GetCategoryByIdAsync(long categoryId)
    {
        CategoryDto? category;

        try
        {
            category = await categoryRepository.GetAll()
                .Select(x => new CategoryDto(x.Id, x.Name, x.Products.Count))
                .FirstOrDefaultAsync(x => x.Id == categoryId);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new BaseResult<CategoryDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError,
            };
        }

        if (category == null)
        {
            logger.LogWarning("Категория с {Id} не найден", categoryId);
            return new BaseResult<CategoryDto>()
            {
                ErrorMessage = ErrorMessage.CategoriesNotFound,
                ErrorCode = (int)ErrorCodes.CategoriesNotFound,
            };
        }

        return new BaseResult<CategoryDto>()
        {
            Data = category,
        };
    }

    public async Task<BaseResult<CategoryDto>> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
    {
        try
        {
            var nullValidation = baseValidator.ValidateOnNull(
                mapper.Map<CreateCategoryDto, Category>(createCategoryDto));
            if (!nullValidation.IsSuccess)
            {
                return new BaseResult<CategoryDto>
                {
                    ErrorMessage = ErrorMessage.EntityNotFound,
                    ErrorCode = (int)ErrorCodes.EntityNotFound,
                };
            }
            
            var category = new Category
            {
                Name = createCategoryDto.Name
            };
            
            await categoryRepository.CreateAsync(category);

            return new BaseResult<CategoryDto>()
            {
                Data = mapper.Map<Category, CategoryDto>(category),
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new BaseResult<CategoryDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError,
            };
        }
    }

    public async Task<BaseResult<CategoryDto>> UpdateCategoryAsync(long categoryId, CategoryDto categoryDto)
    {
        try
        {
            var category = await categoryRepository.GetByIdAsync(categoryId);
        
            var result = baseValidator.ValidateOnNull(category);
            if (!result.IsSuccess)
            {
                return new BaseResult<CategoryDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode,
                };
            }
        
            category.Name = categoryDto.Name;
        
            await categoryRepository.UpdateAsync(category);
            return new BaseResult<CategoryDto>()
            {
                Data = mapper.Map<Category, CategoryDto>(category),
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new BaseResult<CategoryDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError,
            };
        }
    }

    public async Task<BaseResult<CategoryDto>> DeleteCategoryAsync(long categoryId)
    {
        try
        {
            var category = await categoryRepository.GetByIdAsync(categoryId);
        
            var result = baseValidator.ValidateOnNull(category);
            if (!result.IsSuccess)
            {
                return new BaseResult<CategoryDto>()
                {
                    ErrorMessage = result.ErrorMessage,
                    ErrorCode = result.ErrorCode,
                };
            }
        
            await categoryRepository.RemoveAsync(category);
            return new BaseResult<CategoryDto>()
            {
                Data = mapper.Map<Category, CategoryDto>(category),
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new BaseResult<CategoryDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError,
            };
        }
    }
}