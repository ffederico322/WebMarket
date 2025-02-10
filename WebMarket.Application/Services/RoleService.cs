using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebMarket.Application.Resources;
using WebMarket.Domain.Dto.Role;
using WebMarket.Domain.Entity;
using WebMarket.Domain.Enum;
using WebMarket.Domain.Interfaces.Databases;
using WebMarket.Domain.Interfaces.Repositories;
using WebMarket.Domain.Interfaces.Services;
using WebMarket.Domain.Result;

namespace WebMarket.Application.Services;

public class RoleService(
    IUnitOfWork unitOfWork,
    IBaseRepository<User> userRepository,
    IBaseRepository<Role> roleRepository,
    IBaseRepository<UserRole> userRoleRepository,
    IMapper mapper) : IRoleService
{
    public async Task<BaseResult<RoleDto>> CreateRoleAsync(CreateRoleDto createRoleDto)
    {
        var role = await roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == createRoleDto.Name);
        if (role != null)
        {
            return new BaseResult<RoleDto>()
            {
                ErrorMessage = ErrorMessage.RoleAlreadyExists,
                ErrorCode = (int)ErrorCodes.RoleAlreadyExists
            };
        }

        role = new Role()
        {
            Name = createRoleDto.Name,
        };

        await roleRepository.CreateAsync(role);
        return new BaseResult<RoleDto>()
        {
            Data = mapper.Map<RoleDto>(role)
        };
    }

    public async Task<BaseResult<RoleDto>> UpdateRoleAsync(RoleDto roleDto)
    {
        var role = await roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == roleDto.Id);
        if (role != null)
        {
            return new BaseResult<RoleDto>()
            {
                ErrorMessage = ErrorMessage.RoleAlreadyExists,
                ErrorCode = (int)ErrorCodes.RoleAlreadyExists
            };
        }
        
        role.Name = roleDto.Name;
        roleRepository.Update(role);
        await roleRepository.SaveChangesAsync();
        
        return new BaseResult<RoleDto>()
        {
            Data = mapper.Map<RoleDto>(role)
        };
    }

    public async Task<BaseResult<RoleDto>> DeleteRoleAsync(long id)
    {
        var role = await roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
        if (role == null)
        {
            return new BaseResult<RoleDto>()
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = (int)ErrorCodes.RoleNotFound
            };
        }
        
        roleRepository.Remove(role);
        await roleRepository.SaveChangesAsync();
        return new BaseResult<RoleDto>()
        {
            Data = mapper.Map<RoleDto>(role)
        };
    }

    public async Task<BaseResult<UserRoleDto>> AddRoleForUserAsync(UserRoleDto userRoleDto)
    {
        var user = await userRepository.GetAll()
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(x => x.Login == userRoleDto.Login);

        if (user == null)
        {
            return new BaseResult<UserRoleDto>()
            {
                ErrorMessage = ErrorMessage.UserNotFound,
                ErrorCode = (int)ErrorCodes.UserNotFound
            };
        }

        var roles = user.Roles.Select(x => x.Name).ToArray();
        if (roles.All(x => x != userRoleDto.RoleName))
        {
            var role = await roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == userRoleDto.RoleName);
            if (role == null)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorMessage = ErrorMessage.RoleNotFound,
                    ErrorCode = (int)ErrorCodes.RoleNotFound
                };
            }

            UserRole userRole = new UserRole()
            {
                RoleId = role.Id,
                UserId = user.Id,
            };
            await userRoleRepository.CreateAsync(userRole);

            return new BaseResult<UserRoleDto>()
            {
                Data = new UserRoleDto(user.Login, role.Name)
            };
        }

        return new BaseResult<UserRoleDto>()
        {
            ErrorMessage = "Пользователь уже имеет эту роль",
            ErrorCode = 1010
        };
    }

    public async Task<BaseResult<UserRoleDto>> DeleteRoleForUserAsync(UserRoleDto userRoleDto)
    {
        var user = await userRepository.GetAll()
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(x => x.Login == userRoleDto.Login);
        
        if (user == null)
        {
            return new BaseResult<UserRoleDto>()
            {
                ErrorMessage = ErrorMessage.UserNotFound,
                ErrorCode = (int)ErrorCodes.UserNotFound
            };
        }
        
        var role = user.Roles.FirstOrDefault(x => x.Name == userRoleDto.RoleName);
        
        if (role == null)
        {
            return new BaseResult<UserRoleDto>()
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = (int)ErrorCodes.RoleNotFound
            };
        }
        
        var userRole = userRoleRepository.GetAll()
            .Where(x => x.RoleId == role.Id)
            .FirstOrDefault(x => x.UserId == user.Id);
        
        userRoleRepository.Remove(userRole);
        await userRoleRepository.SaveChangesAsync();

        return new BaseResult<UserRoleDto>()
        {
            Data = new UserRoleDto(user.Login, role.Name)
        };
    }

    public async Task<BaseResult<UserRoleDto>> UpdateRoleForUserAsync(UpdateUserRoleDto dto)
    {
        var user = await userRepository.GetAll()
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(x => x.Login == dto.Login);
        if (user == null)
        {
            return new BaseResult<UserRoleDto>()
            {
                ErrorMessage = ErrorMessage.UserNotFound,
                ErrorCode = (int)ErrorCodes.UserNotFound
            };
        }
        
        var role = user.Roles.FirstOrDefault(x => x.Id == dto.FromRoleId);
        if (role == null)
        {
            return new BaseResult<UserRoleDto>()
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = (int)ErrorCodes.RoleNotFound
            };
        }
        
        var newRoleForUser = await roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == dto.ToRoleId);
        if (role == null)
        {
            return new BaseResult<UserRoleDto>()
            {
                ErrorMessage = ErrorMessage.RoleNotFound,
                ErrorCode = (int)ErrorCodes.RoleNotFound
            };
        }
        
        using (var transaction = await unitOfWork.BeginTransactionAsync())
        {
            try
            {
                var userRole = await unitOfWork.UserRoles.GetAll()
                    .Where(x => x.RoleId == role.Id)
                    .FirstAsync(x => x.UserId == user.Id);
            
                unitOfWork.UserRoles.Remove(userRole);
                await unitOfWork.SaveChangesAsync();

                var newUserRole = new UserRole()
                {
                    UserId = user.Id,
                    RoleId = newRoleForUser.Id,
                };
                
                await unitOfWork.UserRoles.CreateAsync(newUserRole);
                await unitOfWork.SaveChangesAsync();
                
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
            }
        }

        return new BaseResult<UserRoleDto>()
        {
            Data = new UserRoleDto(user.Login, newRoleForUser.Name)
        };
    }
}