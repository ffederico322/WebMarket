using WebMarket.Domain.Dto.Role;
using WebMarket.Domain.Entity;
using WebMarket.Domain.Result;

namespace WebMarket.Domain.Interfaces.Services;

/// <summary>
/// Сервис предназаченный для управления ролей
/// </summary>
public interface IRoleService
{
    Task<BaseResult<RoleDto>> CreateRoleAsync(CreateRoleDto createRoleDto);
    
    Task<BaseResult<RoleDto>> UpdateRoleAsync(RoleDto RoleDto);
    
    Task<BaseResult<RoleDto>> DeleteRoleAsync(long id);
    
    Task<BaseResult<UserRoleDto>> AddRoleForUserAsync(UserRoleDto userRoleDto);
    
    Task<BaseResult<UserRoleDto>> DeleteRoleForUserAsync(UserRoleDto userRoleDto);
    
    Task<BaseResult<UserRoleDto>> UpdateRoleForUserAsync (UpdateUserRoleDto dto);
}