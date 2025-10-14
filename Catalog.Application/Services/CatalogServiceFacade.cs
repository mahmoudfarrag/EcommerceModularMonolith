using Catalog.Application.Interfaces;
using Catalog.Application.Services;

public class CatalogServiceFacade : ICatalogServiceFacade
{
    private readonly ProductService _service;

    public CatalogServiceFacade(ProductService service)
    {
        _service = service;
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid id)
    {

        var product = await _service.GetByIdAsync(id);

        return new ProductDto ( product.Id , product.Name,product.Price,product.Stock);
      
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
    {
        //return await _db.Categories
        //    .Where(c => c.Id == id)
        //    .Select(c => new CategoryDto(c.Id, c.Name))
        //    .FirstOrDefaultAsync();

        return null;
    }
}
