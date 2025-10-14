using Microsoft.EntityFrameworkCore;
using Payments.Application.Services;
using Payments.Infrastructure.Persistence;

namespace Payments.Api;

public static class PaymentsModuleRegistration
{
    public static IServiceCollection AddPaymentsModule(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<PaymentsDbContext>(opt => opt.UseSqlServer(
            connectionString,
            sql => sql.MigrationsAssembly(typeof(PaymentsDbContext).Assembly.FullName)
                      .MigrationsHistoryTable("__EFMigrationsHistory", "payments")
        ));

        services.AddScoped<PaymentService>();
        // We'll register an event subscriber (hosted service) below from Host composition root,
        // because we need IServiceProvider to resolve PaymentService per event.
        return services;
    }
}
