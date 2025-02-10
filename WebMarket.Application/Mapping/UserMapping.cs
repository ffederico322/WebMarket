using AutoMapper;
using WebMarket.Domain.Dto.User;
using WebMarket.Domain.Entity;

namespace WebMarket.Application.Mapping;

public class UserMapping : Profile
{
    public UserMapping()
    {
        CreateMap<User, UserDto>().ReverseMap();
    }
}