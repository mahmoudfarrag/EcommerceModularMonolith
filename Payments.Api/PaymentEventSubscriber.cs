using ECommerce.Shared.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Payments.Application.Services;
using Payments.Domain.Entities;


namespace Payments.Api;

// This hosted service subscribes to the in-memory EventBus on StartAsync
// and uses DI to create scopes to handle each event.
public class PaymentEventSubscriber : IHostedService
{
    private readonly IServiceProvider _sp;

    public PaymentEventSubscriber(IServiceProvider sp) => _sp = sp;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Subscribe to the shared OrderPlacedEvent
        EventBus.Subscribe<OrderPlacedEvent>(async evt =>
        {
            // create a scope so we can resolve PaymentService per handling
            using var scope = _sp.CreateScope();
            var payments = scope.ServiceProvider.GetRequiredService<PaymentService>();

            var payment = new Payment(evt.OrderId, evt.Amount);
            await payments.AddAsync(payment);

            Console.WriteLine($"[Payments] Payment recorded for Order {evt.OrderId} Amount {evt.Amount}");
        });

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
