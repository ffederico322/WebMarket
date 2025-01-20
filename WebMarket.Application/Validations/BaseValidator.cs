using WebMarket.Application.Resources;
using WebMarket.Domain.Enum;
using WebMarket.Domain.Interfaces.Validations;
using WebMarket.Domain.Result;

namespace WebMarket.Application.Validations;

public class BaseValidator<T> : IBaseValidator<T> where T : class
{
    public BaseResult ValidateOnNull(T? model)
    {
        if (model == null)
        {
            return new BaseResult()
            {
                ErrorMessage = ErrorMessage.EntityNotFound,
                ErrorCode = (int)ErrorCodes.EntityNotFound
            };
        }
        return new BaseResult();
    }
}