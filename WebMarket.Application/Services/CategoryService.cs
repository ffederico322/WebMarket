﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WebMarket.Application.Resources;
using WebMarket.Domain.Dto.Category;
using WebMarket.Domain.Dto.Product;
using WebMarket.Domain.Entity;
using WebMarket.Domain.Enum;
using WebMarket.Domain.Interfaces.Repositories;
using WebMarket.Domain.Interfaces.Services;
using WebMarket.Domain.Interfaces.Validations;
using WebMarket.Domain.Result;
using WebMarket.Domain.Settings;
using WebMarket.Producer.Interfaces;

namespace WebMarket.Application.Services;

public class CategoryService(
    IBaseRepository<Category> categoryRepository,
    IBaseValidator<Category> baseValidator,
    IMessageProducer messageProducer,
    IOptions<RabbitMqSettings> rabbitMqOptions, 
    ILogger<CategoryService> logger,
    IMapper mapper) : ICategoryService
{
    public async Task<CollectionResult<CategoryDto>> GetAllCategoriesAsync()
    {
        try
        {
            var categories = await categoryRepository.GetAll()
                .ToArrayAsync();
            
            if (!categories.Any())
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
                Data = mapper.Map<List<CategoryDto>>(categories),
                Count = categories.Length
            };
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
    }

    public async Task<BaseResult<CategoryDto>> GetCategoryByIdAsync(long categoryId)
    {
        try
        {
            var category = await categoryRepository.GetByIdAsync(categoryId);
            
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
                Data = mapper.Map<CategoryDto>(category),
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

            messageProducer.SendMessage(category, rabbitMqOptions.Value.RoutingKey, rabbitMqOptions.Value.ExchangeName);
            
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
        
            categoryRepository.Update(category);
            await categoryRepository.SaveChangesAsync();
            
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
        
            categoryRepository.Remove(category);
            await categoryRepository.SaveChangesAsync();
            
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