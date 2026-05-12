
using EZcommerce.Web.Models;
using EZcommerce.Web.Models.ViewModels;
using EZcommerce.Web.Services.Implementations;
using EZcommerce.Web.Tests.Helpers;

namespace EZcommerce.Web.Tests.Services;

public class OrderServiceTests
{
    [Fact]
    public async Task GetAllAsync_WithNoOrders_ReturnEmptyList()
    {
        //Arrange
        var repo = RepositoryHelper.Create<Order>(out var context);
        var service = new OrderService(repo);

        // Act
        var orders = await service.GetAllAsync();

        Assert.Empty(orders);
    }

    [Fact]
    public async Task GetAllAsync_WithOrders_ReturnOrdersList()
    {
        //Arrange
        var repo = RepositoryHelper.Create<Order>(out var context);
        var service = new OrderService(repo);

        context.Orders.AddRange(
            new Order { Id = 1, CustomerName = "bill", CustomerEmail = "a@a.com", ShippingAddressLine1 = "1111 1Ave", ShippingAddressLine2 = "", City = "ZZ", State = "AZ", ZipCode = "91111", Country = "US", TotalAmmount = 1200.00m, Status = "Processing", CreatedAt = new DateTime(2026, 1, 1) },
            new Order { Id = 2, CustomerName = "bill", CustomerEmail = "a@a.com", ShippingAddressLine1 = "1111 1Ave", ShippingAddressLine2 = "", City = "ZZ", State = "AZ", ZipCode = "91111", Country = "US", TotalAmmount = 1200.00m, Status = "Processing", CreatedAt = new DateTime(2026, 1, 1) }

        );

        context.SaveChanges();

        // Act
        var orders = await service.GetAllAsync();

        // Assert
        Assert.NotEmpty(orders);
    }

    [Fact]
    public async Task GetByIdAsync_NoOrders_ReturnNull()
    {
        //Arrange
        var repo = RepositoryHelper.Create<Order>(out var context);
        var service = new OrderService(repo);
        var id = 1;

        // Act
        var order = await service.GetByIdAsync(id);

        // Assert
        Assert.Null(order);
    }

    [Fact]
    public async Task GetByIdAsync_WithOrders_ReturnOrderWithMatchingId()
    {
        //Arrange
        var repo = RepositoryHelper.Create<Order>(out var context);
        var service = new OrderService(repo);
        var id = 1;

        context.Orders.AddRange(
            new Order { Id = 1, CustomerName = "bill", CustomerEmail = "a@a.com", ShippingAddressLine1 = "1111 1Ave", ShippingAddressLine2 = "", City = "ZZ", State = "AZ", ZipCode = "91111", Country = "US", TotalAmmount = 1200.00m, Status = "Processing", CreatedAt = new DateTime(2026, 1, 1) },
            new Order { Id = 2, CustomerName = "bill", CustomerEmail = "a@a.com", ShippingAddressLine1 = "1111 1Ave", ShippingAddressLine2 = "", City = "ZZ", State = "AZ", ZipCode = "91111", Country = "US", TotalAmmount = 1200.00m, Status = "Processing", CreatedAt = new DateTime(2026, 1, 1) }

        );
        context.SaveChanges();

        // Act
        var order = await service.GetByIdAsync(id);

        // Assert
        Assert.NotNull(order);
        Assert.Equal(id, order.Id);
    }

    [Fact]
    public async Task UpdateAsync_WithOrders_UpdateOrderFromDatabase()
    {
        //Arrange
        var repo = RepositoryHelper.Create<Order>(out var context);
        var service = new OrderService(repo);

        context.Orders.AddRange(
            new Order { Id = 1, CustomerName = "bill", CustomerEmail = "a@a.com", ShippingAddressLine1 = "1111 1Ave", ShippingAddressLine2 = "", City = "ZZ", State = "AZ", ZipCode = "91111", Country = "US", TotalAmmount = 1200.00m, Status = "Processing", CreatedAt = new DateTime(2026, 1, 1) },
            new Order { Id = 2, CustomerName = "bill", CustomerEmail = "a@a.com", ShippingAddressLine1 = "1111 1Ave", ShippingAddressLine2 = "", City = "ZZ", State = "AZ", ZipCode = "91111", Country = "US", TotalAmmount = 1200.00m, Status = "Processing", CreatedAt = new DateTime(2026, 1, 1) }
        );
        context.SaveChanges();

        var orderChanges = new Order
        {
            Id = 1,
            CustomerName = "bill2",
            CustomerEmail = "a@a.com",
            ShippingAddressLine1 = "1111 1Ave",
            ShippingAddressLine2 = "",
            City = "ZZ",
            State = "AZ",
            ZipCode = "91111",
            Country = "US",
            TotalAmmount = 1200.00m,
            Status = "Processing",
            CreatedAt = new DateTime(2026, 1, 1)
        };

        // Act
        await service.UpdateAsync(orderChanges);

        var order = context.Orders.FirstOrDefault(i => i.Id == orderChanges.Id);
        // Assert
        Assert.NotNull(order);
        Assert.Equal(order.Id, orderChanges.Id);
        Assert.Equal(order.CustomerName, orderChanges.CustomerName);
    }

        [Fact]
    public async Task UpdateAsync2_WithOrders_UpdateOrderFromDatabase()
    {
        //Arrange
        var repo = RepositoryHelper.Create<Order>(out var context);
        var service = new OrderService(repo);

        context.Orders.AddRange(
            new Order { Id = 1, CustomerName = "bill", CustomerEmail = "a@a.com", ShippingAddressLine1 = "1111 1Ave", ShippingAddressLine2 = "", City = "ZZ", State = "AZ", ZipCode = "91111", Country = "US", TotalAmmount = 1200.00m, Status = "Processing", CreatedAt = new DateTime(2026, 1, 1) },
            new Order { Id = 2, CustomerName = "bill", CustomerEmail = "a@a.com", ShippingAddressLine1 = "1111 1Ave", ShippingAddressLine2 = "", City = "ZZ", State = "AZ", ZipCode = "91111", Country = "US", TotalAmmount = 1200.00m, Status = "Processing", CreatedAt = new DateTime(2026, 1, 1) }
        );
        context.SaveChanges();

        var orderChanges = new OrderViewModel
        {
            Id = 1,
            CustomerName = "bill2",
            TotalAmmount = 1200.00m,
            Status = "Processing",
           
        };

        // Act
        await service.UpdateAsync(orderChanges);

        var order = context.Orders.FirstOrDefault(i => i.Id == orderChanges.Id);
        // Assert
        Assert.NotNull(order);
        Assert.Equal(order.Id, orderChanges.Id);
        Assert.Equal(order.CustomerName, orderChanges.CustomerName);
    }

    [Fact]
    public async Task RemoveAsync_WithOrders_RemoveOrderFromDatabase()
    {
        //Arrange
        var repo = RepositoryHelper.Create<Order>(out var context);
        var service = new OrderService(repo);

        context.Orders.AddRange(
            new Order { Id = 1, CustomerName = "bill", CustomerEmail = "a@a.com", ShippingAddressLine1 = "1111 1Ave", ShippingAddressLine2 = "", City = "ZZ", State = "AZ", ZipCode = "91111", Country = "US", TotalAmmount = 1200.00m, Status = "Processing", CreatedAt = new DateTime(2026, 1, 1) },
            new Order { Id = 2, CustomerName = "bill", CustomerEmail = "a@a.com", ShippingAddressLine1 = "1111 1Ave", ShippingAddressLine2 = "", City = "ZZ", State = "AZ", ZipCode = "91111", Country = "US", TotalAmmount = 1200.00m, Status = "Processing", CreatedAt = new DateTime(2026, 1, 1) }

        );
        context.SaveChanges();
        var id = 1;

        // Act
        await service.RemoveAsync(id);
        var order = context.Orders.FirstOrDefault(i => i.Id == id);

        // Assert
        Assert.Null(order);
    }
}