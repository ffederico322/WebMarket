namespace WebMarket.Domain.Dto.Product;

public record ProductCreateDto(string Name, long CategoryId, string Description, 
    string Image, decimal Price, int Stock);