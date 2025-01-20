namespace WebMarket.Domain.Dto.Product;

public record UpdateProductDto(string Name, long CategoryId, string Description, 
    string Image, decimal Price, int Stock);