﻿using Microsoft.Extensions.DependencyInjection;

namespace WebMarket.Consumer.DependencyInjection;

public static class DependencyInjection
{
    public static void AddConsumer(this IServiceCollection services)
    {
        services.AddHostedService<RabbitMqListener>();
    }
}