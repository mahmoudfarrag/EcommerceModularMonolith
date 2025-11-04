using Ecommerce.Host;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

var conn = configuration.GetConnectionString("DefaultConnection")!;

// Add controllers and explicitly add application parts for controllers in module API assemblies
builder.Services.AddControllers().AddModuleControllers(configuration);


builder.Services.AddHostServices(conn,configuration);

var app = builder.Build();

// Ensure DBs are created (for dev only)
//DatabaseSetupForDeveloment.Setup(app);

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
