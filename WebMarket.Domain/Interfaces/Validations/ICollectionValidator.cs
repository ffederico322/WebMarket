using WebMarket.Domain.Result;

namespace WebMarket.Domain.Interfaces.Validations;

public interface ICollectionValidator
{
    public BaseResult ValidateCollection(params object[] models);
}