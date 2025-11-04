using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Payments.Application.Interfaces;
using Payments.Application.Services;
using Payments.Infrastructure.Persistence;
using Payments.Infrastructure.Repositories;

namespace Payments.Infrastructure.DependencyInjection;

public static class PaymentsModuleRegistration
{
    public static IServiceCollection AddPaymentsModule(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<PaymentsDbContext>(opt => opt.UseSqlServer(
            connectionString,
            sql => sql.MigrationsAssembly(typeof(PaymentsDbContext).Assembly.FullName)
                      .MigrationsHistoryTable(tableName: "__EFMigrationsHistory", "payments")
        ));

        services.AddScoped<PaymentService>();
       // services.AddScoped<IPaymentRepository, PaymentRepository>();
        // We'll register an event subscriber (hosted service) below from Host composition root,
        // because we need IServiceProvider to resolve PaymentService per event.

        services.AddHostedService<PaymentSubscriber>();
        return services;
    }
}
