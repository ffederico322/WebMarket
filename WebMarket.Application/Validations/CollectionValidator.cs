using WebMarket.Application.Resources;
using WebMarket.Domain.Enum;
using WebMarket.Domain.Interfaces.Validations;
using WebMarket.Domain.Result;

namespace WebMarket.Application.Validations;

public class CollectionValidator(IBaseValidator<object> baseValidator) : ICollectionValidator
{
    public BaseResult ValidateCollection(params object[] models)
    {
        if (models.Length == 0)
        {
            return new BaseResult()
            {
                ErrorMessage = ErrorMessage.NoDataProvided,
                ErrorCode = (int)ErrorCodes.NoDataProvided
            };
        }

        foreach (var model in models)
        {
            var validationResult = baseValidator.ValidateOnNull(model);
            if (!validationResult.IsSuccess)
            {
                return validationResult;
            }
        }
        return new BaseResult();
    }
}