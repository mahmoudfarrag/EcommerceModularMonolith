using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Services;
using Orders.Infrastructure.Persistence;
using Orders.Infrastructure.Repositories;
using Orders.Application.Interfaces;

namespace Orders.Api;

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
        return services;
    }
}
