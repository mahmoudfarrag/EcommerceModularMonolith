using Payments.Application.Interfaces;
using Payments.Domain.Entities;
using Payments.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Payments.Infrastructure.Repositories;
public class PaymentRepository : IPaymentRepository
{
    private readonly PaymentsDbContext _db;
    public PaymentRepository(PaymentsDbContext db) => _db = db;

    public async Task AddAsync(Payment p)
    {
        await _db.Payments.AddAsync(p);
        await _db.SaveChangesAsync();
    }
}
