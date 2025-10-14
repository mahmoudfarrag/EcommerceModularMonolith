using Catalog.Application;
using Orders;
using Orders.Application;
using Payments.Application;

var builder = WebApplication.CreateBuilder(args);

// Register MediatR for all modules
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(OrderService).Assembly));

// Register modules
builder.Services.AddCatalogModule(builder.Configuration.GetConnectionString("CatalogDb"));
builder.Services.AddOrdersModule();
builder.Services.AddPaymentsModule();

builder.Services.AddControllers();
var app = builder.Build();

app.MapGet("/", () => "ECommerce Modular Monolith Running");

app.MapPost("/product", async (Catalog.Application.IProductService service, string name, decimal price)
    => await service.CreateAsync(name, price));

app.MapPost("/order", async (Orders.Application.OrderService service, Guid productId)
    => await service.PlaceOrderAsync(productId));

app.Run();
