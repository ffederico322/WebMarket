namespace WebMarket.Domain.Dto.Role;

public record UpdateUserRoleDto(string Login, long FromRoleId, long ToRoleId);