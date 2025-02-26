using System.Text;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using WebMarket.Domain.Settings;
using WebMarket.Producer.Interfaces;

namespace WebMarket.Producer;

public class Producer() : IMessageProducer
{
    public void SendMessage<T>(T message, string routingKey, string? exchange = default)
    {
        var factoty = new ConnectionFactory() {HostName = "localhost"};
        var connection = factoty.CreateConnection();
        using var channel = connection.CreateModel();

        var json = JsonConvert.SerializeObject(message, Formatting.Indented, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });
        var body = Encoding.UTF8.GetBytes(json);
        channel.BasicPublish(exchange, routingKey, body: body);
    }
}