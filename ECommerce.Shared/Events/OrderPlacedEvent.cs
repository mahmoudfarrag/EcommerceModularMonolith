using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Shared.Events;
public class OrderPlacedEvent : IIntegrationEvent
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; private set; } = DateTime.UtcNow;

    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public decimal Amount { get; set; }
}
