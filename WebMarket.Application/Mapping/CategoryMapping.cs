using AutoMapper;
using WebMarket.Domain.Dto.Category;
using WebMarket.Domain.Entity;

namespace WebMarket.Application.Mapping;

public class CategoryMapping : Profile
{
    public CategoryMapping()
    {
        CreateMap<Category, CategoryDto>().ReverseMap();
        CreateMap<Category, CreateCategoryDto>().ReverseMap();
    }
}