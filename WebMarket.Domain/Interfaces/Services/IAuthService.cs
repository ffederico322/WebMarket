using WebMarket.Domain.Dto;
using WebMarket.Domain.Dto.User;
using WebMarket.Domain.Result;

namespace WebMarket.Domain.Interfaces.Services;

/// <summary>
/// Сервис предназначенный для авторизации и регистрации
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="registerUserDto"></param>
    /// <returns></returns>
    Task<BaseResult<UserDto>> Register(RegisterUserDto registerUserDto);
    
    /// <summary>
    /// Авторизация пользователя 
    /// </summary>
    /// <param name="loginUserDto"></param>
    /// <returns></returns>
    Task<BaseResult<TokenDto>> Login(LoginUserDto loginUserDto);
}