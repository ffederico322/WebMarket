using AutoMapper;
using WebMarket.Application.Mapping;

namespace WebMarket.Tests.Configurations;

public static class MapperConfiguration
{
    public static IMapper GetMapperConfiguration()
    {
        var mockMapper = new AutoMapper.MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ProductMapping());
        });
        return mockMapper.CreateMapper();
    }

}