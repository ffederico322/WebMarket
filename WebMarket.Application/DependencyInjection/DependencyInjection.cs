using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using WebMarket.Application.Mapping;
using WebMarket.Application.Services;
using WebMarket.Application.Validations;
using WebMarket.Application.Validations.FluentValidations;
using WebMarket.Application.Validations.FluentValidations.CartItemValidation;
using WebMarket.Application.Validations.FluentValidations.CartValidation;
using WebMarket.Application.Validations.FluentValidations.CategoryValidation;
using WebMarket.Application.Validations.FluentValidations.OrderItemValidation;
using WebMarket.Application.Validations.FluentValidations.OrderValidation;
using WebMarket.Application.Validations.FluentValidations.Report;
using WebMarket.Application.Validations.ProductValidators;
using WebMarket.Domain.Dto.Cart;
using WebMarket.Domain.Dto.CartItem;
using WebMarket.Domain.Dto.Category;
using WebMarket.Domain.Dto.Order;
using WebMarket.Domain.Dto.OrderItem;
using WebMarket.Domain.Dto.Product;
using WebMarket.Domain.Interfaces;
using WebMarket.Domain.Interfaces.Services;
using WebMarket.Domain.Interfaces.Validations;

namespace WebMarket.Application.DependencyInjection;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ProductMapping));
        
        InitServices(services);
    }

    private static void InitServices(IServiceCollection services)
    {
        // Register base validators
        services.AddScoped(typeof(IBaseValidator<>), typeof(BaseValidator<>));
        services.AddScoped<ICollectionValidator, CollectionValidator>();
        
        services.AddScoped<IProductValidator, ProductValidator>();
        
        // FluentValidation registrations
        services.AddScoped<IValidator<CreateProductDto>, CreateProductValidator>();
        services.AddScoped<IValidator<UpdateProductDto>, UpdateProductValidator>();
        
        services.AddScoped<IValidator<CreateCategoryDto>, CreateCategoryValidator>();
        services.AddScoped<IValidator<UpdateCategoryDto>, UpdateCategoryValidator>();
        
        services.AddScoped<IValidator<CartDto>, CartValidator>();
        services.AddScoped<IValidator<CartItemDto>, CartItemValidator>();
        
        services.AddScoped<IValidator<OrderDto>, OrderValidator>();
        services.AddScoped<IValidator<OrderItemDto>, OrderItemValidator>();
        
        // Service registrations
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IRoleService, RoleService>();
    }
}