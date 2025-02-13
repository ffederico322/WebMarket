﻿namespace WebMarket.Producer.Interfaces;

public interface IMessageProducer
{
    void SendMessage<T>(T message, string routingKey, string? exchange = default);
}