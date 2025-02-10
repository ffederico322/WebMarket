using Microsoft.AspNetCore.Mvc;
using WebMarket.Domain.Dto;
using WebMarket.Domain.Dto.User;
using WebMarket.Domain.Interfaces.Services;
using WebMarket.Domain.Result;

namespace WebMarket.Api.Controllers;

[ApiController]
public class AuthController(
    IAuthService authService) : ControllerBase
{
    /// <summary>
    /// Регистрация пользователя 
    /// </summary>
    /// <param name="registerUserDto"></param>
    /// <returns></returns>
    [HttpPost("Register")]
    public async Task<ActionResult<BaseResult<UserDto>>> Register([FromBody] RegisterUserDto registerUserDto)
    {
        var response = await authService.Register(registerUserDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }

    /// <summary>
    /// Авторизация пользователя 
    /// </summary>
    /// <param name="loginUserDto"></param>
    /// <returns></returns>
    [HttpPost("Login")]
    public async Task<ActionResult<BaseResult<TokenDto>>> Login([FromBody] LoginUserDto loginUserDto)
    {
        var response = await authService.Login(loginUserDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}