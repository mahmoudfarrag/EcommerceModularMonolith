using ECommerce.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Payments.Application.Interfaces;
using Payments.Application.Services;
using Payments.Infrastructure.Persistence;
using Payments.Infrastructure.Repositories;

namespace Payments.ModuleDefinition;
[DependsOn(typeof(Orders.ModuleDefinition.OrdersModule))]
public class PaymentsModule : IModule
{
 
    public void Register(IServiceCollection services, string connectionString)
    {
        services.AddDbContext<PaymentsDbContext>(opt => opt.UseSqlServer(
          connectionString,
          sql => sql.MigrationsAssembly(typeof(PaymentsDbContext).Assembly.FullName)
                    .MigrationsHistoryTable("__EFMigrationsHistory", "payments")
      ));
        services.AddScoped<IPaymentRepository, PaymentRepository>();

        services.AddScoped<PaymentService>();
       
        // We'll register an event subscriber (hosted service) below from Host composition root,
        // because we need IServiceProvider to resolve PaymentService per event.

        services.AddHostedService<PaymentSubscriber>();
       
    }
}
