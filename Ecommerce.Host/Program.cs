using Catalog.Api;
using Orders.Api;
using Payments.Api;
using Catalog.Infrastructure.Persistence;
using Orders.Infrastructure.Persistence;
using Payments.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Payments.Api; // for PaymentEventSubscriber
using Catalog.Api.Controllers;
using Orders.Api.Controllers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var conn = configuration.GetConnectionString("DefaultConnection")!;




// Add controllers and explicitly add application parts for controllers in module API assemblies
builder.Services.AddControllers()
    .AddApplicationPart(typeof(ProductController).Assembly)
    .AddApplicationPart(typeof(OrderController).Assembly);

// Register Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ECommerce API", Version = "v1" });
});

// Register each module (DbContexts + services)
builder.Services.AddCatalogModule(conn);
builder.Services.AddOrdersModule(conn);
builder.Services.AddPaymentsModule(conn);

// Register the Payments hosted subscriber so it can resolve PaymentService when events arrive
builder.Services.AddHostedService<PaymentEventSubscriber>();

var app = builder.Build();

// Ensure DBs are created (for dev only)
using (var scope = app.Services.CreateScope())
{
    var s = scope.ServiceProvider;

    var catalogDb = s.GetRequiredService<CatalogDbContext>();
    catalogDb.Database.Migrate(); // runs migrations for catalog (if created)

    var ordersDb = s.GetRequiredService<OrdersDbContext>();
    ordersDb.Database.Migrate();

    var paymentsDb = s.GetRequiredService<PaymentsDbContext>();
    paymentsDb.Database.Migrate();

    // seed if empty
    if (!catalogDb.Products.Any())
    {
        catalogDb.Products.AddRange(
            new Catalog.Domain.Entities.Product("Laptop", 1200m, 10),
            new Catalog.Domain.Entities.Product("Headphones", 150m, 20)
        );
        catalogDb.SaveChanges();
    }
}
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce API v1");
    });
}
app.MapControllers();
app.Run();
