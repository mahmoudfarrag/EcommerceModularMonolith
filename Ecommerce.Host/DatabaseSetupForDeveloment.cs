//using Catalog.Infrastructure.Persistence;
//using Microsoft.EntityFrameworkCore;
//using Orders.Infrastructure.Persistence;
//using Payments.Infrastructure.Persistence;

//namespace Ecommerce.Host;

//public static class DatabaseSetupForDeveloment
//{
//    public static void Setup(WebApplication app)
//    {
//        using (var scope = app.Services.CreateScope())
//        {
//            var s = scope.ServiceProvider;

//            var catalogDb = s.GetRequiredService<CatalogDbContext>();
//            catalogDb.Database.Migrate(); // runs migrations for catalog (if created)

//            var ordersDb = s.GetRequiredService<OrdersDbContext>();
//            ordersDb.Database.Migrate();

//            var paymentsDb = s.GetRequiredService<PaymentsDbContext>();
//            paymentsDb.Database.Migrate();

//            // seed if empty
//            if (!catalogDb.Products.Any())
//            {
//                catalogDb.Products.AddRange(
//                    new Catalog.Domain.Entities.Product("Laptop", 1200m, 10),
//                    new Catalog.Domain.Entities.Product("Headphones", 150m, 20)
//                );
//                catalogDb.SaveChanges();
//            }
//        }
//    }
//}
