using EZcommerce.Web.Data;
using EZcommerce.Web.Models;
using EZcommerce.Web.Models.Session;
using EZcommerce.Web.Models.ViewModels;
using EZcommerce.Web.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace EZcommerce.Web.Services.Implementations;

public class EZcommerceService : IEZcommerceService
{
    private readonly EZcommerceRepository _repo;
    public EZcommerceService(EZcommerceRepository repo)
    {
        _repo = repo;
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
        if (!await _repo.ProductAnyAsync(item.ProductId))
            throw new Exception("Product does not exits");
    }

    private async Task ValidatePrice(CartItem item)
    {
        var product = await _repo.ProductGetByIdAsync(item.ProductId);

        if (product!.Price != item.PriceSnapshot)
            throw new Exception("Price mismatch");
    }
    private async Task ValidateInventory(CartItem item)
    {
        var product = await _repo.ProductGetByIdWithInventoryAsync(item.ProductId);

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
        await _repo.OrderAddAndSaveAsync(order);

        return order.Id;
    }

    public async Task LowerInventoriesByCartItems(List<CartItem> items)
    {
        foreach (var item in items)
        {
            var inventory = await _repo.InventoryGetByProductIdAsync(item.ProductId);
            if (inventory == null)
                throw new Exception("Inventory not exits error");
            inventory.Quantity -= item.Quantity;

        }
        await _repo.SaveChangesAsync();
    }

    public async Task<List<Order>> OrderGetAllAsync()
    {
        var orders = await _repo.OrderGetAllAsync();
        return orders;
    }

    public async Task<Order?> OrderGetByIdAsync(int id)
    {
        var order = await _repo.OrderGetByIdAsync(id);
        return order;
    }

    public async Task OrderInventoryRollback(int orderId)
    {
        if (!await _repo.OrderAnyAsync(orderId))
            throw new Exception("Order not exits in OrderInventoryRollback function");

        var orderItems = await _repo.OrderItemGetByOrderIdWithProductAndInventoryAsync(orderId);

        foreach (var item in orderItems)
        {
            item.Product!.Inventory!.Quantity += item.Quantity;
        }
        await _repo.SaveChangesAsync();
    }

    public async Task OrderRemove(int orderId)
    {
        var order = await _repo.OrderGetByIdWithTrackingAsync(orderId) ?? throw new Exception();
        await _repo.OrderRemoveAndSaveAsync(order);
    }

    public async Task OrderUpdate(Order orderChanges)
    {
        var order = await _repo.OrderGetByIdWithTrackingAsync(orderChanges.Id);
        if (order is null)
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

        await _repo.SaveChangesAsync();
    }

    public async Task OrderUpdateAsync(OrderViewModel model)
    {
        var order = await _repo.OrderGetByIdWithTrackingAsync(model.Id);
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

        await _repo.SaveChangesAsync();
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        return await _repo.ProductGetAllAsync();
    }

    public async Task<List<Product>> ProductGetAllIncludeInventoryAsync()
    {
        return await _repo.ProductGetAllWithInventoryAsync(); ;
    }

    public async Task<Product?> ProductGetWithInventoryAsync(int id)
    {
        return await _repo.ProductGetByIdWithInventoryAsync(id);
    }

    public async Task<Product?> ProductGetbyIdWithInventoryAndCategoryAsync(int id)
    {
        return await _repo.ProductGetByIdWithInventoryAndCategoryAsync(id);
    }

    public async Task ProductCreateWithInventory(ProductCreateViewModel model)
    {
        var product = new Product
        {
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            ImageUrl = model.ImageUrl,
            CategoryId = model.CategoryId,
            Created_at = DateTime.UtcNow,
            Inventory = new Inventory { Quantity = model.InventoryQuantity }
        };
        await _repo.ProductAddAndSaveAsync(product);
    }

    public async Task ProductEditWithInventory(ProductCreateViewModel model)
    {
        var product = await _repo.ProductGetByIdWithInventoryWithTrackingAsync(model.Id);
        if (product is null)
            throw new Exception();

        product.Name = model.Name;
        product.Description = model.Description;
        product.Price = model.Price;
        product.ImageUrl = model.ImageUrl;
        product.CategoryId = model.CategoryId;
        product.Inventory!.Quantity = model.InventoryQuantity;

        await _repo.SaveChangesAsync();

    }

    public async Task ProductRemove(int id)
    {
        var product = await _repo.ProductGetByIdWithTrackingAsync(id) ?? throw new Exception();
        await _repo.ProductRemoveAndSaveAsync(product);
    }

    public async Task<List<Payment>> PaymentGetAllAsync()
    {
        return await _repo.PaymentGetAllAsync();
    }

    public async Task<Payment?> PaymentGetByIdAsync(int id)
    {
        return await _repo.PaymentGetByIdAsync(id);
    }

    public async Task PaymentEditAsync(Payment payment)
    {
        var oldPayment = await _repo.PaymentGetByIdWithTrackingAsync(payment.Id) ?? throw new Exception();
        oldPayment.OrderId = payment.OrderId;
        oldPayment.Amount = payment.Amount;
        oldPayment.Method = payment.Method;
        oldPayment.Status = payment.Status;
        oldPayment.TransactionReference = payment.TransactionReference;

        await _repo.SaveChangesAsync();
    }


    public async Task PaymentCreate(Payment payment)
    {
        await _repo.PaymentAddAndSaveAsync(payment);
    }

    public async Task PaymentRemove(int id)
    {
        var payment = await _repo.PaymentGetByIdWithTrackingAsync(id) ?? throw new Exception();
        await _repo.PaymentRemoveAndSaveAsync(payment);
    }

    public async Task<List<Category>> CategoryGetAllAsync()
    {
        return await _repo.CategoryGetAllAsync();
    }

}
