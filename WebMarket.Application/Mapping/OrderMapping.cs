using AutoMapper;
using WebMarket.Domain.Dto.Order;
using WebMarket.Domain.Dto.OrderItem;
using WebMarket.Domain.Entity;

namespace WebMarket.Application.Mapping;

public class OrderMapping : Profile
{
    public OrderMapping()
    {
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<OrderItem, OrderItemDto>().ReverseMap();
    }
}