namespace WebMarket.Domain.Dto.Product;

public record ProductDto(long Id, string Name, long CategoryId, string Description, string Image, 
    decimal Price);
