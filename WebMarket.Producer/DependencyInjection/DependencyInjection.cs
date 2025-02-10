using Microsoft.Extensions.DependencyInjection;
using WebMarket.Producer.Interfaces;

namespace WebMarket.Producer.DependencyInjection;

public static class DependencyInjection
{
    public static void AddProducer(this IServiceCollection services)
    {
        services.AddScoped<IMessageProducer, Producer>();
    }
}