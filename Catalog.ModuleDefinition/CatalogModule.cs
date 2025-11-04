using Catalog.Application.Interfaces;
using Catalog.Application.Services;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Repositories;
using ECommerce.Shared.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog.ModuleDefinition;
public class CatalogModule : IModule
{
    public void Register(IServiceCollection services, string connectionString)
    {
        // Configure DbContext with schema-specific Migrations history table (schema: catalog)
        services.AddDbContext<CatalogDbContext>(opt => opt.UseSqlServer(
            connectionString,
            sql => sql.MigrationsAssembly(typeof(CatalogDbContext).Assembly.FullName)
                      .MigrationsHistoryTable("__EFMigrationsHistory", "catalog")
        ));

        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ProductService>();
        services.AddScoped<ICatalogServiceFacade, CatalogServiceFacade>();
    }
}
