
using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;
using System.Text.Json;

namespace Orders.Infrastructure.Persistence;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options) { }
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("orders");

        modelBuilder.Entity<Order>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Amount).HasPrecision(18, 2);
            b.Property(x => x.CreatedAt);
        });

        modelBuilder.Entity<OutboxMessage>().ToTable("OutboxMessages", "orders");

        base.OnModelCreating(modelBuilder);
    }
   

}
