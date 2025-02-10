using AutoMapper;
using WebMarket.Domain.Dto.Role;
using WebMarket.Domain.Entity;

namespace WebMarket.Application.Mapping;

public class RoleMapping : Profile
{
    public RoleMapping ()
    {
        CreateMap<Role, RoleDto>().ReverseMap();
    }
}