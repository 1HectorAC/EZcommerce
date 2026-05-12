
using EZcommerce.Areas.Admin.Controllers;
using EZcommerce.Web.Models;
using EZcommerce.Web.Models.ViewModels;
using EZcommerce.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EZcommerce.Web.Tests.Areas.Admin.Controllers;

public class OrderControllerTests
{
    [Fact]
    public async Task Index_WhenOrdersExits_ReturnViewWithOrders()
    {
        // Arrange
        var mockOrderService = new Mock<IOrderService>();
        mockOrderService.Setup(s => s.GetAllAsync())
            .ReturnsAsync(new List<Order>
            {
                new Order { Id = 1, CustomerName = "bill", CustomerEmail = "a@a.com", ShippingAddressLine1 = "1111 1Ave", ShippingAddressLine2 = "", City = "ZZ", State = "AZ", ZipCode = "91111", Country = "US", TotalAmmount = 1200.00m, Status = "Processing", CreatedAt = new DateTime(2026, 1, 1) },
                new Order { Id = 2, CustomerName = "bill", CustomerEmail = "a@a.com", ShippingAddressLine1 = "1111 1Ave", ShippingAddressLine2 = "", City = "ZZ", State = "AZ", ZipCode = "91111", Country = "US", TotalAmmount = 1200.00m, Status = "Processing", CreatedAt = new DateTime(2026, 1, 1) }
            });
        var controller = new OrderController(mockOrderService.Object);

        // Act
        var result = await controller.Index();

        //Assert
        var view = Assert.IsType<ViewResult>(result);
        var orders = Assert.IsType<List<Order>>(view.Model);
        Assert.Equal(2, orders.Count());
    }
    [Fact]
    public async Task Edit_WhenOrderIdExits_ReturnViewWithOrderViewModel()
    {
        // Arrange
        var mockOrderService = new Mock<IOrderService>();

        mockOrderService.Setup(s => s.GetByIdAsync(1))
        .ReturnsAsync(
            new Order
            {
                Id = 1,
                CustomerName = "bill",
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
            }

        );
        int id = 1;

        var controller = new OrderController(mockOrderService.Object);

        // Act
        var result = await controller.Edit(id);

        // Assert
        var view = Assert.IsType<ViewResult>(result);
        var order = Assert.IsType<OrderViewModel>(view.Model);
        Assert.Equal(id, order.Id);
    }

    [Fact]
    public async Task EditPost_WhenModelIsValid_ReturnRedirectToActionResult()
    {
        // Arrange
        var mockOrderService = new Mock<IOrderService>();
        mockOrderService.Setup(s => s.UpdateAsync(It.IsAny<OrderViewModel>()));
        var controller = new OrderController(mockOrderService.Object);

        // Act
        OrderViewModel model = new OrderViewModel
        {
            Id = 1,
            CustomerName = "bill2",
            TotalAmmount = 1200.00m,
            Status = "Processing",

        };
        var result = await controller.Edit(model);
        // Assert
        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public async Task Delete_WhenOrderIdExits_ReturnViewWithOrder()
    {
        // Arrange
        var mockOrderService = new Mock<IOrderService>();

        mockOrderService.Setup(s => s.GetByIdAsync(1))
        .ReturnsAsync(
            new Order { Id = 1, CustomerName = "bill", CustomerEmail = "a@a.com", ShippingAddressLine1 = "1111 1Ave", ShippingAddressLine2 = "", City = "ZZ", State = "AZ", ZipCode = "91111", Country = "US", TotalAmmount = 1200.00m, Status = "Processing", CreatedAt = new DateTime(2026, 1, 1) }
        );
        int id = 1;

        var controller = new OrderController(mockOrderService.Object);

        // Act
        var result = await controller.Delete(id);
        // Assert
        var view = Assert.IsType<ViewResult>(result);
        var order = Assert.IsType<Order>(view.Model);
        Assert.Equal(id, order.Id);
    }

    [Fact]
    public async Task DeleteConfirmed_WhenOrderIdExits_ReturnRedirectToActionResult()
    {
        // Arrange
        var mockOrderService = new Mock<IOrderService>();
        mockOrderService.Setup(s => s.RemoveAsync(1));
        var controller = new OrderController(mockOrderService.Object);
        var id = 1;
        // Act
        var result = await controller.DeleteConfirmed(id);

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
    }
}