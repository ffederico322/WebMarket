using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebMarket.Application.Resources;
using WebMarket.Domain.Dto;
using WebMarket.Domain.Dto.Product;
using WebMarket.Domain.Dto.User;
using WebMarket.Domain.Entity;
using WebMarket.Domain.Enum;
using WebMarket.Domain.Interfaces.Databases;
using WebMarket.Domain.Interfaces.Repositories;
using WebMarket.Domain.Interfaces.Services;
using WebMarket.Domain.Result;

namespace WebMarket.Application.Services;

public class AuthService(
    IUnitOfWork unitOfWork,
    IBaseRepository<User> userRepository,
    IBaseRepository<UserToken> userTokenRepository,
    IBaseRepository<Role> roleRepository,
    ITokenService tokenService,
    ILogger<AuthService> logger, 
    IMapper mapper) : IAuthService
{
    public async Task<BaseResult<UserDto>> Register(RegisterUserDto registerUserDto)
    {
        if (registerUserDto.Password != registerUserDto.PasswordConfirm)
        {
            return new BaseResult<UserDto>()
            {
                ErrorMessage = ErrorMessage.PasswordNotEqualsPasswordConfirm,
                ErrorCode = (int)ErrorCodes.PasswordNotEqualsPassworConfirm
            };
        }

        var user = await userRepository.GetAll()
            .FirstOrDefaultAsync(x => x.Login == registerUserDto.Login);
        if (user != null)
        {
            return new BaseResult<UserDto>()
            {
                ErrorMessage = ErrorMessage.UserAlreadyExists,
                ErrorCode = (int)ErrorCodes.UserAlreadyExists
            };
        }
        var hashUserPassword = HashPassword(registerUserDto.Password);

        // Добавьте перед вызовом метода
        if (unitOfWork == null)
            throw new InvalidOperationException("UnitOfWork is not initialized");
        
        logger.LogInformation("Starting registration process");
        logger.LogInformation($"UnitOfWork is null: {unitOfWork == null}");
        logger.LogInformation($"Users repository is null: {unitOfWork?.Users == null}");
        
        using (var transaction = await unitOfWork.BeginTransactionAsync())
        {
            try
            {
                user = new User()
                {
                    Login = registerUserDto.Login,
                    Password = hashUserPassword,
                    Email = registerUserDto.Email,
                };
                
                if (user == null)
                    throw new ArgumentNullException(nameof(user));
                
                if (unitOfWork.Users == null)
                    throw new InvalidOperationException("Users repository is not initialized");
                
                await unitOfWork.Users.CreateAsync(user);
                
                await unitOfWork.SaveChangesAsync();
                
                var role = await roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == nameof(Roles.User));
                if (role == null)
                {
                    return new BaseResult<UserDto>()
                    {
                        ErrorMessage = ErrorMessage.RoleNotFound,
                        ErrorCode = (int)ErrorCodes.RoleNotFound
                    };
                }
                
                UserRole userRole = new UserRole()
                {
                    UserId = user.Id,
                    RoleId = role.Id
                };
                
                await unitOfWork.UserRoles.CreateAsync(userRole);
                
                await unitOfWork.SaveChangesAsync();
                
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
            }
        }

        return new BaseResult<UserDto>()
        {
            Data = mapper.Map<UserDto>(user),
        };
    }

    public async Task<BaseResult<TokenDto>> Login(LoginUserDto loginUserDto)
    {
        try
        {
            var user = userRepository.GetAll()
                .Include(x => x.Roles)
                .FirstOrDefault(x => x.Login == loginUserDto.Login);
            if (user == null)
            {
                return new BaseResult<TokenDto>()
                {
                    ErrorMessage = ErrorMessage.UserNotFound,
                    ErrorCode = (int)ErrorCodes.UserNotFound
                };
            }
            if (!IsVerifablePassword(user.Password, loginUserDto.Password))
            {
                return new BaseResult<TokenDto>()
                {
                    ErrorMessage = ErrorMessage.PasswordIsWrong,
                    ErrorCode = (int)ErrorCodes.PasswordIsWrong
                };
            }
             
            var userToken = await userTokenRepository.GetAll().FirstOrDefaultAsync(x => x.UserId == user.Id);

            var userRoles = user.Roles;
            var claims = userRoles.Select(x => new Claim(ClaimTypes.Role, x.Name)).ToList();
            claims.Add(new Claim(ClaimTypes.Name, user.Login));
            var accesToken = tokenService.GenerateAccessToken(claims);
            var refreshToken = tokenService.GenerateRefreshToken();
            
            if (userToken == null)
            {
                userToken = new UserToken()
                {
                    UserId = user.Id,
                    RefreshToken = refreshToken,
                    RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
                };
                
                await userTokenRepository.CreateAsync(userToken);
            }
            else
            {
                userToken.RefreshToken = refreshToken;
                userToken.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                userTokenRepository.Update(userToken);
                await userTokenRepository.SaveChangesAsync();
                
            }

            return new BaseResult<TokenDto>()
            {
                Data = new TokenDto()
                {
                    AccessToken = accesToken,
                    RefreshToken = refreshToken,
                }
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            return new BaseResult<TokenDto>()
            {
                ErrorMessage = ErrorMessage.InternalServerError,
                ErrorCode = (int)ErrorCodes.InternalServerError,
            };
        }
    }

    private string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(bytes).ToLower().Replace("-", "");
    }

    private bool IsVerifablePassword(string userPasswordHash, string userPassword)
    {
        var hash = HashPassword(userPassword);
        return hash == userPasswordHash;
    }
}