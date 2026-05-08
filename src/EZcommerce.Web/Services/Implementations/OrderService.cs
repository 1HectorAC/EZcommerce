
using EZcommerce.Web.Models;
using EZcommerce.Web.Models.ViewModels;
using EZcommerce.Web.Repositories;
using EZcommerce.Web.Services;
using Microsoft.EntityFrameworkCore;

namespace EZcommerce.Web.Services.Implementations;

public class OrderService : IOrderService
{

    private readonly IGenericRepository<Order> _orderRepo;

    public OrderService(IGenericRepository<Order> orderRepo)
    {
        _orderRepo = orderRepo;
    }
    public async Task<List<Order>> GetAllAsync()
    {
        return await _orderRepo
            .Query()
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<Order?> GetByIdAsync(int id)
    {
        return await _orderRepo.GetByIdAsync(id);
    }
    public async Task UpdateAsync(Order order)
    {
        var oldOrder = await _orderRepo.GetByIdAsync(order.Id);
        if (oldOrder is null)
        {
            throw new Exception("OrderUpdateAsync: Order does not exits.");
        }
        oldOrder.CustomerName = order.CustomerName ?? oldOrder.CustomerName;
        oldOrder.CustomerEmail = order.CustomerEmail ?? oldOrder.CustomerEmail;
        oldOrder.CustomerPhone = order.CustomerPhone ?? oldOrder.CustomerPhone;
        oldOrder.ShippingAddressLine1 = order.ShippingAddressLine1 ?? oldOrder.ShippingAddressLine1;
        oldOrder.ShippingAddressLine2 = order.ShippingAddressLine2 ?? oldOrder.ShippingAddressLine2;
        oldOrder.City = order.City ?? oldOrder.City;
        oldOrder.State = order.State ?? oldOrder.State;
        oldOrder.ZipCode = order.ZipCode ?? oldOrder.ZipCode;
        oldOrder.Country = order.Country ?? oldOrder.Country;
        oldOrder.Status = order.Status ?? oldOrder.Status;

        await _orderRepo.SaveChangesAsync();
    }

    public async Task UpdateAsync(OrderViewModel model)
    {
        var order = await _orderRepo.GetByIdAsync(model.Id);
        if (order is null)
            throw new Exception();

        order.CustomerName = model.CustomerName ?? order.CustomerName;
        order.CustomerEmail = model.CustomerEmail ?? order.CustomerEmail;
        order.CustomerPhone = model.CustomerPhone ?? order.CustomerPhone;
        order.ShippingAddressLine1 = model.ShippingAddressLine1 ?? order.ShippingAddressLine1;
        order.ShippingAddressLine2 = model.ShippingAddressLine2 ?? order.ShippingAddressLine2;
        order.City = model.City ?? order.City;
        order.State = model.State ?? order.State;
        order.ZipCode = model.ZipCode ?? order.ZipCode;
        order.Country = model.Country ?? order.Country;
        order.Status = model.Status ?? order.State;

        await _orderRepo.SaveChangesAsync();
    }
    public async Task RemoveAsync(int orderId)
    {
        var order = await _orderRepo.GetByIdAsync(orderId) ?? throw new Exception();
        _orderRepo.Remove(order);
        await _orderRepo.SaveChangesAsync();
    }

}