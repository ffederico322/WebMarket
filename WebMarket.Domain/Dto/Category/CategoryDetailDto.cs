using WebMarket.Domain.Dto.Product;

namespace WebMarket.Domain.Dto.Category;

public record CategoryDetailDto(long Id, string Name, IEnumerable<ProductDto> Products);