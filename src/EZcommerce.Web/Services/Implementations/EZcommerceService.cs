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
}
