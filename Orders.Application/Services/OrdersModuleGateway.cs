using Catalog.Domain.Entities;
using ECommerce.Shared.Events;
using Orders.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Services;
internal class OrdersModuleGateway
{
    public static async Task  ProcessAfterOrderCreated(OrderPlacedEvent orderPlacedEventData)
    {
        await EventBus.PublishAsync(orderPlacedEventData);
    }

}
