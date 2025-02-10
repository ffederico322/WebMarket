using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WebMarket.Domain.Dto;
using WebMarket.Domain.Entity;
using WebMarket.Domain.Interfaces.Repositories;
using WebMarket.Domain.Interfaces.Services;
using WebMarket.Domain.Result;
using WebMarket.Domain.Settings;

namespace WebMarket.Application.Services;

public class TokenService(IOptions<JwtSettings> options, IBaseRepository<User> userRepository) : ITokenService
{
    private readonly string _jwtKey = options.Value.JwtKey;
    private readonly string _issuer = options.Value.Issuer;
    private readonly string _audience = options.Value.Audience;

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var securityToken = new JwtSecurityToken(_issuer, _audience, claims, null, DateTime.Now.AddMinutes(10), credentials);
        var token =new JwtSecurityTokenHandler().WriteToken(securityToken);
        return token;
    }
    
    

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        randomNumberGenerator.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey)),
            ValidateLifetime = true,
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, 
            out SecurityToken securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, 
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
        
        return claimsPrincipal;
    }
    
    public async Task<BaseResult<TokenDto>> RefreshTokenAsync(TokenDto tokenDto)
    {
        var accessToken = tokenDto.AccessToken;
        var refreshToken = tokenDto.RefreshToken;
        
        var claimsPrincipal = GetPrincipalFromExpiredToken(accessToken);
        var userName = claimsPrincipal.Identity?.Name;
        
        var user = await userRepository.GetAll()
            .Include(x => x.UserToken)
            .FirstOrDefaultAsync(x => x.Login == userName);
        if (user == null || user.UserToken.RefreshToken != refreshToken ||
            user.UserToken.RefreshTokenExpiryTime < DateTime.UtcNow)
        {
            return new BaseResult<TokenDto>()
            {
                // можно потом добавить в ErrorMessage
                ErrorMessage = "Invalid refresh token"
            };
        }
        var newAccessToken = GenerateAccessToken(claimsPrincipal.Claims);
        var newRefreshToken = GenerateRefreshToken();

        user.UserToken.RefreshToken = newRefreshToken;
        userRepository.Update(user);
        await userRepository.SaveChangesAsync();

        return new BaseResult<TokenDto>()
        {
            Data = new TokenDto()
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
            }
        };

    }
}

