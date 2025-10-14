namespace Orders.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProductId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private Order() { }

    public Order(Guid productId, decimal amount)
    {
        ProductId = productId;
        Amount = amount;
    }
}
