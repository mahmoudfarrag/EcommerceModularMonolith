using ECommerce.Shared.Events;
using Orders.Application.Interfaces;
using Orders.Domain.Entities;
using Orders.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Orders.Infrastrcture.Repositories;
public class OutboxRepository : IOutboxRepository
{
    private readonly OrdersDbContext _dbContext;

    public OutboxRepository(OrdersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(IIntegrationEvent @event)
    {
        var message = new OutboxMessage
        {
            Type = @event.GetType().AssemblyQualifiedName!,
            Content = JsonSerializer.Serialize(@event)
        };

        await _dbContext.OutboxMessages.AddAsync(message);
    }
}
