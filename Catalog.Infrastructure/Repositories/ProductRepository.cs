using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Repositories;
public class ProductRepository : IProductRepository
{
    private readonly CatalogDbContext _db;
    public ProductRepository(CatalogDbContext db) => _db = db;

    public async Task AddAsync(Product p) => await _db.Products.AddAsync(p);
    public async Task<IEnumerable<Product>> GetAllAsync() => await _db.Products.AsNoTracking().ToListAsync();
    public async Task<Product?> GetByIdAsync(Guid id) => await _db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
}
