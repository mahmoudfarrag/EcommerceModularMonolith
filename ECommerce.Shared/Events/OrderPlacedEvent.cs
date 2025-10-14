using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Events;
public sealed class OrderPlacedEvent : IEvent
{
    public Guid OrderId { get; init; }
    public Guid ProductId { get; init; }
    public decimal Amount { get; init; }
}
