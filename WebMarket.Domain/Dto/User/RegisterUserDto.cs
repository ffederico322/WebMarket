namespace WebMarket.Domain.Dto.User;

public record RegisterUserDto(string Login, string Password, string PasswordConfirm, string Email);