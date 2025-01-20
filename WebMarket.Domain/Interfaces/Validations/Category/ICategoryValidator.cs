using System.ComponentModel.DataAnnotations;
using WebMarket.Domain.Entity;
using WebMarket.Domain.Result;

namespace WebMarket.Domain.Interfaces.Validations;

public interface ICategoryValidator : IBaseValidator<Category>
{
    public BaseResult Validate(Category category);
}