using AutoMapper;
using WebMarket.Domain.Dto.Product;
using WebMarket.Domain.Entity;

namespace WebMarket.Application.Mapping;

public class ProductMapping : Profile
{
    public ProductMapping()
    {
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}