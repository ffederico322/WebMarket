namespace WebMarket.Domain.Dto.Cart;

public record CartDto(int Id, decimal TotalPrice, long UserId);