using Catalog.Application.Interfaces;
using ECommerce.Shared;
using ECommerce.Shared.Events;
using Orders.Application.GatewayInterfaces;
using Orders.Application.Interfaces;
using Orders.Domain.Entities;


namespace Orders.Application.Services;

public class OrderService
{
    private readonly IOrderRepository _orders;
    private readonly IOrdersInboundGateway _inboundGateway;
    private readonly IOrdersOutboundGateway _outboundGateway;
    private readonly IOutboxRepository _outbox;

    public OrderService(IOrderRepository orders,
                        IOrdersInboundGateway inboundGateway,
                        IOrdersOutboundGateway outboundGateway,
                        IOutboxRepository outbox)
    {
        _orders = orders;
        _inboundGateway = inboundGateway;
        _outboundGateway = outboundGateway;
        _outbox = outbox;
    }

    public async Task<IEnumerable<Order>> GetAllAsync() => await _orders.GetAllAsync();

    public async Task<Guid> PlaceOrderAsync(Guid productId)
    {
        var product = await _inboundGateway.GetProductAsync(productId) ?? throw new InvalidOperationException("Product not found");
        var order = new Order(productId, product.Price);

        await _orders.AddAsync(order);
        // Create event
        var orderPlaced = new OrderPlacedEvent
        {
            OrderId = order.Id,
            ProductId = productId,
            Amount = 30
        };
        // Save event to Outbox (same DbContext transaction)
        await _outbox.AddAsync(orderPlaced);


        await _orders.SaveChangesAsync(); // transaction for outbox and order


        // publish strongly-typed event in Shared
     //  await _outboundGateway.ProcessAfterOrderCreated(order);

        return order.Id;
    }
}
