// Orders.Application.Gateways.Inbound.OrdersInboundGateway.cs
using Catalog.Application.Interfaces;
using Orders.Application.GatewayInterfaces;

public class OrdersInboundGateway : IOrdersInboundGateway
{
    private readonly ICatalogServiceFacade _catalogFacade;
    // later you could inject IPaymentFacade, IShippingFacade, etc.

    public OrdersInboundGateway(ICatalogServiceFacade catalogFacade)
    {
        _catalogFacade = catalogFacade;
    }

    public async Task<ProductDto?> GetProductAsync(Guid id)
        => await _catalogFacade.GetProductByIdAsync(id);

    public async Task<CategoryDto?> GetCategoryAsync(Guid id)
        => await _catalogFacade.GetCategoryByIdAsync(id);
}
