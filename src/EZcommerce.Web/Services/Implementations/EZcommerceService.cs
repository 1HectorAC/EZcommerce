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
    private readonly IGenericRepository<Payment> _paymentRepo;
    private readonly IGenericRepository<Inventory> _inventoryRepo;
    private readonly IGenericRepository<Category> _categoryRepo;
    private readonly IGenericRepository<OrderItem> _orderItemRepo;

    public EZcommerceService(
         IGenericRepository<Product> productRepo,
         IGenericRepository<Order> orderRepo,
         IGenericRepository<Payment> paymentRepo,
         IGenericRepository<Inventory> inventoryRepo,
         IGenericRepository<Category> categoryRepo,
         IGenericRepository<OrderItem> orderItemRepo)
    {
        _productRepo = productRepo;
        _orderRepo = orderRepo;
        _paymentRepo = paymentRepo;
        _inventoryRepo = inventoryRepo;
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
        await _orderRepo.AddAsync(order);
        await _orderRepo.SaveChangesAsync();

        return order.Id;
    }

    public async Task LowerInventoriesByCartItems(List<CartItem> items)
    {
        foreach (var item in items)
        {
            var inventory = await _inventoryRepo
                .Query()
                .FirstOrDefaultAsync(i => i.ProductId == item.ProductId)
                ?? throw new Exception("Inventory not exits");

            inventory.Quantity -= item.Quantity;
        }
        await _inventoryRepo.SaveChangesAsync();
    }


    public async Task<List<Product>> ProductGetAllWithInventoryAndCategoryAsync()
    {
        return await _productRepo.Query()
            .AsNoTracking()
            .Include(i => i.Inventory)
            .Include(i => i.Category)
            .ToListAsync();
    }
    public async Task<Product?> ProductGetByIdWithInventoryAndCategoryAsync(int id)
    {
        return await _productRepo.Query()
            .AsNoTracking()
            .Include(i => i.Inventory)
            .Include(i => i.Category)
            .FirstOrDefaultAsync(i => i.Id == id);
    }
    public async Task ProductAndInventoryAddAsync(ProductCreateViewModel model)
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
        await _productRepo.AddAsync(product);
        await _productRepo.SaveChangesAsync();
    }
    public async Task ProductAndInventoryUpdateAsync(ProductCreateViewModel model)
    {
        var product = await _productRepo.Query()
            .Include(i => i.Inventory)
            .FirstOrDefaultAsync(i => i.Id == model.Id);

        if (product is null)
            throw new Exception();

        product.Name = model.Name;
        product.Description = model.Description;
        product.Price = model.Price;
        product.ImageUrl = model.ImageUrl;
        product.CategoryId = model.CategoryId;
        product.Inventory!.Quantity = model.InventoryQuantity;

        await _productRepo.SaveChangesAsync();
    }
    public async Task ProductRemoveAsync(int id)
    {
        var product = await _productRepo.GetByIdAsync(id) ?? throw new Exception();

        _productRepo.Remove(product);
        await _productRepo.SaveChangesAsync();
    }


    public async Task<List<Order>> OrderGetAllAsync()
    {
        return await _orderRepo
            .Query()
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Order?> OrderGetByIdAsync(int id)
    {
        return await _orderRepo.GetByIdAsync(id);
    }

    public async Task OrderInventoryRollbackAsync(int orderId)
    {
        if (!await _orderRepo.Query().AsNoTracking().AnyAsync(i => i.Id == orderId))
            throw new Exception("Order not exits in OrderInventoryRollback function");

        var orderItems = await _orderItemRepo
            .Query()
            .Include(i => i.Product)
            .ThenInclude(j => j!.Inventory)
            .Where(i => i.OrderId == orderId)
            .ToListAsync();

        foreach (var item in orderItems)
        {
            item.Product!.Inventory!.Quantity += item.Quantity;
        }
        await _orderRepo.SaveChangesAsync();
    }
    public async Task OrderUpdateAsync(Order orderChanges)
    {
        var order = await _orderRepo.GetByIdAsync(orderChanges.Id);
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

        await _orderRepo.SaveChangesAsync();
    }

    public async Task OrderUpdateAsync(OrderViewModel model)
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
        public async Task OrderRemoveAsync(int orderId)
    {
        var order = await _orderRepo.GetByIdAsync(orderId) ?? throw new Exception();
        _orderRepo.Remove(order);
        await _orderRepo.SaveChangesAsync();
    }


    public async Task<List<Payment>> PaymentGetAllAsync()
    {
        return await _paymentRepo
            .Query()
            .AsNoTracking()
            .ToListAsync();
    }
    public async Task<Payment?> PaymentGetByIdAsync(int id)
    {
        return await _paymentRepo
            .Query()
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);
    }
    public async Task PaymentAddAsync(Payment payment)
    {
        await _paymentRepo.AddAsync(payment);
        await _paymentRepo.SaveChangesAsync();
    }
    public async Task PaymentUpdateAsync(Payment payment)
    {
        var oldPayment = await _paymentRepo.GetByIdAsync(payment.Id) ?? throw new Exception();
        oldPayment.OrderId = payment.OrderId;
        oldPayment.Amount = payment.Amount;
        oldPayment.Method = payment.Method;
        oldPayment.Status = payment.Status;
        oldPayment.TransactionReference = payment.TransactionReference;

        await _paymentRepo.SaveChangesAsync();
    }
    public async Task PaymentRemoveAsync(int id)
    {
        var payment = await _paymentRepo.GetByIdAsync(id) ?? throw new Exception();

        _paymentRepo.Remove(payment);
        await _paymentRepo.SaveChangesAsync();
    }

    public async Task<List<Category>> CategoryGetAllAsync()
    {
        return await _categoryRepo.Query().AsNoTracking().ToListAsync();
    }

}
