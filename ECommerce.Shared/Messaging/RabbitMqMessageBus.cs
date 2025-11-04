using ECommerce.Shared.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace ECommerce.Shared.Messaging;
public class RabbitMqMessageBus : IMessageBus
{
    private readonly IConnection _connection;

    public RabbitMqMessageBus(IConnection connection)
    {
        _connection = connection;
    }

    public Task PublishAsync(IIntegrationEvent @event)
    {
        using var channel = _connection.CreateModel();

        var eventName = @event.GetType().Name;
        channel.QueueDeclare(eventName, durable: true, exclusive: false, autoDelete: false);

        var message = JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish("", eventName, null, body);
        Console.WriteLine($"Published event {eventName} to RabbitMQ");

        return Task.CompletedTask;
    }
}
