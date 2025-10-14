using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;


namespace Order.Infrastructure;



public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options) { }
    public DbSet<Order> Orders => Set<Order>();
}
