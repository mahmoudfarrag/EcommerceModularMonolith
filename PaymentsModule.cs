using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Payments.Application;
using Payments.Infrastructure;

namespace Payments;

public static class PaymentsModule
{
    public static IServiceCollection AddPaymentsModule(this IServiceCollection services)
    {
        services.AddDbContext<PaymentsDbContext>(opt => opt.UseInMemoryDatabase("PaymentsDb"));
        services.AddScoped<PaymentProcessor>();
        services.AddHostedService<OutboxProcessor>();
        return services;
    }
}
