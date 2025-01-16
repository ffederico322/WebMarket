using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebMarket.DAL.Interceptors;
using WebMarket.DAL.Repositories;
using WebMarket.Domain.Entity;
using WebMarket.Domain.Interfaces.Repositories;

namespace WebMarket.DAL.DependencyInjection;

public static class DependencyInjection
{
    public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MSSQL");

        services.AddSingleton<DateInterceptor>();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        services.InitRepositories();
    }

    private static void InitRepositories(this IServiceCollection services)
    {
        services.AddScoped<IBaseRepository<User>, BaseRepository<User>>();
        services.AddScoped<IBaseRepository<Order>, BaseRepository<Order>>();
        services.AddScoped<IBaseRepository<Product>, BaseRepository<Product>>();
        services.AddScoped<IBaseRepository<Category>, BaseRepository<Category>>();
        services.AddScoped<IBaseRepository<Cart>, BaseRepository<Cart>>();
        services.AddScoped<IBaseRepository<CartItem>, BaseRepository<CartItem>>();
        services.AddScoped<IBaseRepository<OrderItem>, BaseRepository<OrderItem>>();
    }
}