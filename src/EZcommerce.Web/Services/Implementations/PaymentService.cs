
using EZcommerce.Web.Models;
using EZcommerce.Web.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EZcommerce.Web.Services.Implementations;

public class PaymentService : IPaymentService
{
    private readonly IGenericRepository<Payment> _paymentRepo;
    public PaymentService(IGenericRepository<Payment> paymentRepo)
    {
        _paymentRepo = paymentRepo;
    }
    public async Task<List<Payment>> GetAllAsync()
    {
        return await _paymentRepo
            .Query()
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<Payment?> GetByIdAsync(int id)
    {
        return await _paymentRepo
            .Query()
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);
    }
    public async Task AddAsync(Payment payment)
    {
        await _paymentRepo.AddAsync(payment);
        await _paymentRepo.SaveChangesAsync();
    }
    public async Task UpdateAsync(Payment payment)
    {
        var oldPayment = await _paymentRepo.GetByIdAsync(payment.Id) ?? throw new Exception();
        oldPayment.OrderId = payment.OrderId;
        oldPayment.Amount = payment.Amount;
        oldPayment.Method = payment.Method;
        oldPayment.Status = payment.Status;
        oldPayment.TransactionReference = payment.TransactionReference;

        await _paymentRepo.SaveChangesAsync();
    }
    public async Task RemoveAsync(int id)
    {
        var payment = await _paymentRepo.GetByIdAsync(id) ?? throw new Exception();

        _paymentRepo.Remove(payment);
        await _paymentRepo.SaveChangesAsync();
    }

}