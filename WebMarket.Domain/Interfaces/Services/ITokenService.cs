using System.Security.Claims;
using WebMarket.Domain.Dto;
using WebMarket.Domain.Result;

namespace WebMarket.Domain.Interfaces.Services;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    
    string GenerateRefreshToken();
    
    ClaimsPrincipal GetPrincipalFromExpiredToken(string accesstoken);
    
    Task<BaseResult<TokenDto>> RefreshTokenAsync(TokenDto tokenDto);
}