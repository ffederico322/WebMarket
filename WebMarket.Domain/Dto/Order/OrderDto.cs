namespace WebMarket.Domain.Dto.Order;

public record OrderDto(int Id, bool IsActive, decimal TotalPrice, 
    long UserId, long CartId, string CreatedAt, string UpdatedAt);