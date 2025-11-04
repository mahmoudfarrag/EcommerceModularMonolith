using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Services;
using Orders.Infrastructure.Persistence;
using Orders.Infrastructure.Repositories;
using Orders.Application.Interfaces;
using Orders.Infrastrcture.Repositories;
using Orders.Infrastrcture.Outbox;
using Orders.Application.GatewayInterfaces;
using Orders.Application.Gateways;

namespace Orders.Infrastructure.DependencyInjection;

public static class OrdersModuleRegistration
{
    public static IServiceCollection AddOrdersModule(this IServiceCollection services, string connectionString)
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
        return services;
    }
}
