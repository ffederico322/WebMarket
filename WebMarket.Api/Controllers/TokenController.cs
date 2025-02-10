using Microsoft.AspNetCore.Mvc;
using WebMarket.Domain.Dto;
using WebMarket.Domain.Interfaces.Services;
using WebMarket.Domain.Result;

namespace WebMarket.Api.Controllers;

[ApiController]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="tokenService"></param>
    public TokenController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost]
    [Route("refreshToken")]
    public async Task<ActionResult<BaseResult<TokenDto>>> RefreshToken([FromBody] TokenDto tokenDto)
    {
        var response = await _tokenService.RefreshTokenAsync(tokenDto);
        if (response.IsSuccess)
        {
            return Ok(response);
        }
        return BadRequest(response);
    }
}