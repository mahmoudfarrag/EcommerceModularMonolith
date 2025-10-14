using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities;
public class Product
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; } = default!;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }

    private Product() { } // EF ctor

    public Product(string name, decimal price, int stock)
    {
        Name = name;
        Price = price;
        Stock = stock;
    }

    public void ReduceStock(int qty)
    {
        if (qty <= 0) throw new ArgumentException(nameof(qty));
        if (qty > Stock) throw new InvalidOperationException("Not enough stock");
        Stock -= qty;
    }
}
