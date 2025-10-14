using Catalog.Application;
using Catalog.Infrastructure;
using Microsoft.EntityFrameworkCore;


namespace Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services, string conn)
    {
        services.AddDbContext<CatalogDbContext>(opt => opt.UseInMemoryDatabase("CatalogDb"));
        services.AddScoped<IProductService, ProductService>();
        return services;
    }
}
