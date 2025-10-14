namespace Payments.Domain.Entities;

public class Payment
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public bool Completed { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    private Payment() { }

    public Payment(Guid orderId, decimal amount)
    {
        OrderId = orderId;
        Amount = amount;
        Completed = true;
    }
}
