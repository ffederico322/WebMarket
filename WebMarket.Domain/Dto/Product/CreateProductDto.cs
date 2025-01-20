namespace WebMarket.Domain.Dto.Product;

public record CreateProductDto(string Name, long CategoryId, string Description, 
    string Image, decimal Price, int Stock);