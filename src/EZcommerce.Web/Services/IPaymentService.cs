using EZcommerce.Web.Models;

namespace EZcommerce.Web.Services;

public interface IPaymentService
{
    Task<List<Payment>> GetAllAsync();
    Task<Payment?> GetByIdAsync(int id);
    Task AddAsync(Payment payment);
    Task UpdateAsync(Payment payment);
    Task RemoveAsync(int id);
}