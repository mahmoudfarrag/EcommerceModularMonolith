using ECommerce.Shared.Events;
using Orders.Application.GatewayInterfaces;
using Orders.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Gateways;
public class OrdersOutboundGateway : IOrdersOutboundGateway
{
    public async Task ProcessAfterOrderCreated(Order order)
    {
        var orderPlacedEvent = new OrderPlacedEvent
        {
            OrderId = order.Id,
            ProductId = order.ProductId,
            Amount = order.Amount
        };

        await EventBus.PublishAsync(orderPlacedEvent);
    }
}
