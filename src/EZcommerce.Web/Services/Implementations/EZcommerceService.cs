using EZcommerce.Web.Data;
using EZcommerce.Web.Models;
using EZcommerce.Web.Models.Session;
using Microsoft.EntityFrameworkCore;

namespace EZcommerce.Web.Services.Implementations;

public class EZcommerceService : IEZcommerceService
{

    // Change to use EZcommerce repo later
    private readonly EZcommerceDbContext _context;
    public EZcommerceService(EZcommerceDbContext context)
    {
        _context = context;
    }


    // maybe put validations in OrderValidation class
    public void ValidateCart(List<CartItem> items)
    {
        foreach (var item in items)
        {
            ValidateProductExists(item);
            ValidatePrice(item);
            ValidateInventory(item);
        }
    }
    private void ValidateProductExists(CartItem item)
    {
        if (!_context.Products.Any(p => p.Id == item.ProductId))
            throw new Exception("Product does not exits");
    }

    private void ValidatePrice(CartItem item)
    {
        var product = _context.Products
            .FirstOrDefault(i => i.Id == item.ProductId);

        if (product!.Price != item.PriceSnapshot)
            throw new Exception("Price mismatch");
    }
    private void ValidateInventory(CartItem item)
    {
        var product = _context.Products
            .Include(i => i.Inventory)
            .FirstOrDefault(i => i.Id == item.ProductId);

        if (product!.Inventory!.Quantity < item.Quantity)
            throw new Exception("Not enough Inventory");
    }

    public async Task<int> InitiateOrderFromCartItems(List<CartItem> items)
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
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        return order.Id;
    }

    public async Task LowerInventoriesByCartItems(List<CartItem> items)
    {
        foreach(var item in items){
            var inventory = _context.Inventories.FirstOrDefault(i => i.ProductId == item.ProductId);
            if(inventory == null)
                throw new Exception("Inventory not exits error");
            inventory.Quantity -= item.Quantity;

        }
        await _context.SaveChangesAsync();
    }

    public async Task OrderInventoryRollback(int orderId)
    {
        var order = _context.Orders.FirstOrDefault(i => i.Id == orderId);
        if (order is null)
            throw new Exception("Order not exits in OrderInventoryRollback function");
        var orderItems = _context.OrderItems
            .Include(i => i.Product)
            .ThenInclude(j => j!.Inventory)
            .Where(i => i.OrderId == orderId);
        foreach(var item in orderItems)
        {
            item.Product!.Inventory!.Quantity += item.Quantity;
        }
        _context.Orders.Remove(order);
    }

    public void OrderRemove(int orderId)
    {
        var order = _context.Orders.FirstOrDefault(i => i.Id == orderId);
        if(order is null)
        {
            throw new Exception("OrderRemove: Order does not exits.");
        }
        _context.Orders.Remove(order);
    }

    public void OrderUpdate(int orderId, Order orderChanges)
    {
        var order = _context.Orders.FirstOrDefault(i => i.Id == orderId);
        if(order is null)
        {
            throw new Exception("OrderUpdate: Order does not exits.");
        }
        order.CustomerName = orderChanges.CustomerName ?? order.CustomerName;
        order.CustomerEmail = orderChanges.CustomerEmail ?? order.CustomerEmail;
        order.CustomerPhone = orderChanges.CustomerPhone ?? order.CustomerPhone;
        order.ShippingAddressLine1 = orderChanges.ShippingAddressLine1 ?? order.ShippingAddressLine1;
        order.ShippingAddressLine2 = orderChanges.ShippingAddressLine2 ?? order.ShippingAddressLine2;
        order.City = orderChanges.City ?? order.City;
        order.State = orderChanges.State ?? order.State;
        order.ZipCode = orderChanges.ZipCode ?? order.ZipCode;
        order.Country = orderChanges.Country ?? order.Country;
        order.Status = orderChanges.Status ?? order.Status;

        _context.SaveChanges();
    }

    public void PaymentCreate(Payment payment)
    {
        _context.Add(payment);
        _context.SaveChanges();
    }

    public async Task<List<Product>> ProductGetAllIncludeInventoryAsync()
    {
        var products = await _context.Products
            .AsNoTracking()
            .Include(i => i.Inventory)
            .ToListAsync();
        return products;
    }

     public async Task<List<Category>> CategoryGetAllAsync()
    {
        var categories = await _context.Categories
            .AsNoTracking().ToListAsync();

        return categories;
    }


}
