using Payments.Domain.Entities;
using Payments.Infrastructure.Persistence;

namespace Payments.Application.Services;

public class PaymentService
{
    private readonly PaymentsDbContext _db;
    public PaymentService(PaymentsDbContext db) => _db = db;

    public async Task AddAsync(Payment p)
    {
        await _db.Payments.AddAsync(p);
        await _db.SaveChangesAsync();
    }
}
