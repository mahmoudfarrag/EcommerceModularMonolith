using Microsoft.EntityFrameworkCore;
using Orders.Application.Interfaces;
using Orders.Domain.Entities;
using Orders.Infrastructure.Persistence;

namespace Orders.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly OrdersDbContext _db;
    public OrderRepository(OrdersDbContext db) => _db = db;

    public async Task AddAsync(Order order) => await _db.Orders.AddAsync(order);
    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();
    public async Task<IEnumerable<Order>> GetAllAsync() => await _db.Orders.AsNoTracking().ToListAsync();
}
