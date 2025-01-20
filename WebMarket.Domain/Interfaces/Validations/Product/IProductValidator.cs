using WebMarket.Domain.Entity;
using WebMarket.Domain.Result;

namespace WebMarket.Domain.Interfaces.Validations;

public interface IProductValidator
{
    /// <summary>
    /// Проверяется корректность введенных значений параметра продукта 
    /// </summary>
    /// <param name="product"></param>
    /// <param name="category"></param>
    /// <returns></returns>
    Task <BaseResult> CreateValidator(Product product, Category category);
}