using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application;
using Orders.Infrastructure;

namespace Orders;

public static class OrdersModule
{
    public static IServiceCollection AddOrdersModule(this IServiceCollection services)
    {
        services.AddDbContext<OrdersDbContext>(opt => opt.UseInMemoryDatabase("OrdersDb"));
        services.AddScoped<OrderService>();
        return services;
    }
}
