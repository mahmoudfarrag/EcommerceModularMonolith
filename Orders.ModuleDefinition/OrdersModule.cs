using ECommerce.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.GatewayInterfaces;
using Orders.Application.Gateways;
using Orders.Application.Interfaces;
using Orders.Application.Services;
using Orders.Infrastrcture.Outbox;
using Orders.Infrastrcture.Repositories;
using Orders.Infrastructure.Persistence;
using Orders.Infrastructure.Repositories;

namespace Orders.ModuleDefinition;
[DependsOn(typeof(Catalog.ModuleDefinition.CatalogModule))]
public class OrdersModule : IModule
{

    public void Register(IServiceCollection services, string connectionString)
    {
        services.AddDbContext<OrdersDbContext>(opt => opt.UseSqlServer(
       connectionString,
       sql => sql.MigrationsAssembly(typeof(OrdersDbContext).Assembly.FullName)
                 .MigrationsHistoryTable("__EFMigrationsHistory", "orders")
   ));

        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<OrderService>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();
        services.AddScoped<IOrdersInboundGateway, OrdersInboundGateway>();
        services.AddScoped<IOrdersOutboundGateway, OrdersOutboundGateway>();
        //IOrdersInboundGateway

        services.AddHostedService<OutboxProcessor>();
        
    }
}