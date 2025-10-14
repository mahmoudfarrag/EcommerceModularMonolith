using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Events;
// Simple in-memory typed event bus for domain events.
// Subscribe<T> stores handlers; PublishAsync<T> invokes them.
// Note: this is fine for in-process modular monolith examples.
// For production, consider an event mediator or durable outbox for cross-process delivery.
public static class EventBus
{
    private static readonly ConcurrentDictionary<Type, List<Func<object, Task>>> _handlers
        = new();

    public static void Subscribe<TEvent>(Func<TEvent, Task> handler) where TEvent : IEvent
    {
        var list = _handlers.GetOrAdd(typeof(TEvent), _ => new List<Func<object, Task>>());
        list.Add(evt => handler((TEvent)evt));
    }

    public static async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
    {
        if (_handlers.TryGetValue(typeof(TEvent), out var handlers))
        {
            await Task.WhenAll(handlers.Select(h => h(@event)));
        }
    }
}