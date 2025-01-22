using AutoMapper;
using WebMarket.Domain.Dto.Cart;
using WebMarket.Domain.Dto.CartItem;
using WebMarket.Domain.Dto.Order;
using WebMarket.Domain.Entity;

namespace WebMarket.Application.Mapping;

public class CartMapping : Profile
{
    public CartMapping()
    {
        CreateMap<Cart, CartDto>().ReverseMap();
        CreateMap<CartItem, CartItemDto>().ReverseMap();
    }
}