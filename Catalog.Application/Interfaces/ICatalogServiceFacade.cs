using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Interfaces;
public interface ICatalogServiceFacade
{
    Task<ProductDto?> GetProductByIdAsync(Guid id);
    Task<CategoryDto?> GetCategoryByIdAsync(Guid id);
}

public record ProductDto(Guid Id, string Name, decimal Price, int Stock);
public record CategoryDto(Guid Id, string Name);
