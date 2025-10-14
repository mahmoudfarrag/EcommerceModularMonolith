using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Catalog.Application.Services;

public class ProductService
{
    private readonly IProductRepository _repo;
    public ProductService(IProductRepository repo) => _repo = repo;

    public Task<IEnumerable<Product>> GetAllAsync() => _repo.GetAllAsync();
    public Task<Product?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);

    public async Task<Guid> CreateAsync(string name, decimal price, int stock)
    {
        var p = new Product(name, price, stock);
        await _repo.AddAsync(p);
        await _repo.SaveChangesAsync();
        return p.Id;
    }
}
