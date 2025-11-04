using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using Payments.Application.Services;
using Payments.Domain.Entities;
using ECommerce.Shared.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Payments.Application.Services;

public class PaymentSubscriber : BackgroundService
{
    private readonly IServiceProvider _sp;
    private readonly IConnection _connection;

    public PaymentSubscriber(IServiceProvider sp, IConnection connection)
    {
        _sp = sp;
        _connection = connection;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = _connection.CreateModel();
        channel.QueueDeclare("OrderPlacedEvent", durable: true, exclusive: false, autoDelete: false);

        // ✅ Use the async consumer since DispatchConsumersAsync = true
        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.Received += async (ch, ea) =>
        {
            try
            {
                var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                var evt = JsonSerializer.Deserialize<OrderPlacedEvent>(json);

                using var scope = _sp.CreateScope();
                var paymentService = scope.ServiceProvider.GetRequiredService<PaymentService>();

                await paymentService.AddAsync(new Payment(evt!.OrderId, evt.Amount));

                Console.WriteLine($"[Payments] Payment created for Order {evt.OrderId}");

                channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Payments] Error processing message: {ex.Message}");
                // Optional: you can Nack or requeue the message here
            }
        };

        channel.BasicConsume(queue: "OrderPlacedEvent",
                             autoAck: false,
                             consumer: consumer);

        return Task.CompletedTask;
    }
}
