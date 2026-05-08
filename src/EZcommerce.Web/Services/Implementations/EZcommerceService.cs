using EZcommerce.Web.Data;
using EZcommerce.Web.Models;
using EZcommerce.Web.Models.Session;
using EZcommerce.Web.Models.ViewModels;
using EZcommerce.Web.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EZcommerce.Web.Services.Implementations;

public class EZcommerceService : IEZcommerceService
{
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IGenericRepository<Order> _orderRepo;
    private readonly IGenericRepository<Category> _categoryRepo;
    private readonly IGenericRepository<OrderItem> _orderItemRepo;

    public EZcommerceService(
         IGenericRepository<Product> productRepo,
         IGenericRepository<Order> orderRepo,
         IGenericRepository<Category> categoryRepo,
         IGenericRepository<OrderItem> orderItemRepo)
    {
        _productRepo = productRepo;
        _orderRepo = orderRepo;
        _categoryRepo = categoryRepo;
        _orderItemRepo = orderItemRepo;
    }

    // maybe put validations in OrderValidation class
    public async Task ValidateCart(List<CartItem> items)
    {
        foreach (var item in items)
        {
            await ValidateProductExists(item);
            await ValidatePrice(item);
            await ValidateInventory(item);
        }
    }
    private async Task ValidateProductExists(CartItem item)
    {
        if (!await _productRepo.Query().AsNoTracking().AnyAsync(i => i.Id == item.ProductId))
            throw new Exception("Product does not exits");
    }

    private async Task ValidatePrice(CartItem item)
    {
        var product = await _productRepo.GetByIdAsync(item.ProductId);

        if (product!.Price != item.PriceSnapshot)
            throw new Exception("Price mismatch");
    }
    private async Task ValidateInventory(CartItem item)
    {
        var product = await _productRepo
            .Query()
            .AsNoTracking()
            .Include(i => i.Inventory)
            .FirstOrDefaultAsync(i => i.Id == item.ProductId);

        if (product!.Inventory!.Quantity < item.Quantity)
            throw new Exception("Not enough Inventory");
    }

    public async Task<int> OrderAndOrderItemsAddFromCartItemsAsync(List<CartItem> items)
    {
        List<OrderItem> orderItems = new List<OrderItem>();
        foreach (var item in items)
        {
            orderItems.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                PriceAtPurchase = item.PriceSnapshot
            });
        }

        var totalPrice = Math.Round(items.Sum(i => i.PriceSnapshot * i.Quantity), 2);
        var order = new Order
        {
            CustomerName = "",
            CustomerEmail = "",
            CustomerPhone = "",
            ShippingAddressLine1 = "",
            ShippingAddressLine2 = "",
            City = "",
            State = "",
            ZipCode = "",
            Country = "",
            TotalAmmount = totalPrice,
            Status = "Processing",
            CreatedAt = DateTime.UtcNow,
            OrderItems = orderItems
        };
        await _orderRepo.AddAsync(order);
        await _orderRepo.SaveChangesAsync();

        return order.Id;
    }

    public async Task AddQuantitiesToInventoriesFromOrderAsync(int orderId)
    {
        var orderItems = await _orderItemRepo
            .Query()
            .Include(i => i.Product)
            .ThenInclude(j => j!.Inventory)
            .Where(i => i.OrderId == orderId)
            .ToListAsync();

        if (orderItems.Count <= 0)
            throw new Exception("AddQuantitiesToInventoriesFromOrderAsync: no orderItems exits iwth orderId");

        foreach (var item in orderItems)
        {
            item.Product!.Inventory!.Quantity += item.Quantity;
        }
        await _orderItemRepo.SaveChangesAsync();
    }
    public async Task SubtractQuantitiesToInventoriesFromOrderAsync(int orderId)
    {
        var orderItems = await _orderItemRepo
                .Query()
                .Include(i => i.Product)
                .ThenInclude(j => j!.Inventory)
                .Where(i => i.OrderId == orderId)
                .ToListAsync();

        if (orderItems.Count <= 0)
            throw new Exception("SubtractQuantitiesToInventoriesFromOrderAsync: no orderItems exits iwth orderId");

        foreach (var item in orderItems)
        {
            item.Product!.Inventory!.Quantity -= item.Quantity;
        }
        await _orderItemRepo.SaveChangesAsync();
    }

    public async Task<List<Category>> CategoryGetAllAsync()
    {
        return await _categoryRepo.Query().AsNoTracking().ToListAsync();
    }

}
