using ECommerce.Shared.Events;
using ECommerce.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orders.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Orders.Infrastrcture.Outbox;
public class OutboxProcessor : BackgroundService
{
    private readonly IServiceProvider _sp;

    public OutboxProcessor(IServiceProvider sp)
    {
        _sp = sp;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
            var bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();

            var messages = await db.OutboxMessages
                .Where(x => x.ProcessedOnUtc == null)
                .OrderBy(x => x.OccurredOnUtc)
                .Take(10)
                .ToListAsync(stoppingToken);

            foreach (var msg in messages)
            {
                try
                {
                    var type = Type.GetType(msg.Type)!;
                    
                    var @event = (IIntegrationEvent)JsonSerializer.Deserialize(msg.Content, type)!;

                    await bus.PublishAsync(@event);

                    msg.ProcessedOnUtc = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    msg.Error = ex.Message;
                }
            }

            await db.SaveChangesAsync(stoppingToken);
            await Task.Delay(5000, stoppingToken);
        }
    }
}
