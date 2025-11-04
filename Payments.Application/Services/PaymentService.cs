using Payments.Application.Interfaces;
using Payments.Domain.Entities;


namespace Payments.Application.Services;

public class PaymentService
{
    private readonly IPaymentRepository _db;

    public PaymentService(IPaymentRepository db)
    {
        _db = db;
    }

    public async Task AddAsync(Payment p)
    {
        await _db.AddAsync(p);
       
    }
}
