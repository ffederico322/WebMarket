using WebMarket.Domain.Dto.Category;
using WebMarket.Domain.Result;

namespace WebMarket.Domain.Interfaces.Services;

/// <summary>
/// Сервис отвечает за работу доменной части категории (Category)
/// </summary>
public interface ICategoryService
{
    Task<CollectionResult<CategoryDto>> GetAllCategoriesAsync();

    Task<BaseResult<CategoryDto>> GetCategoryByIdAsync(long categoryId);
    
    Task<BaseResult<CategoryDto>> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
    
    Task<BaseResult<CategoryDto>> UpdateCategoryAsync(long categoryId, CategoryDto categoryDto);
    
    Task<BaseResult<CategoryDto>> DeleteCategoryAsync(long categoryId);
}