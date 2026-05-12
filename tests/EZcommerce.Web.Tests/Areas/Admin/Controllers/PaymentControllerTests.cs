
using EZcommerce.Areas.Admin.Controllers;
using EZcommerce.Web.Models;
using EZcommerce.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EZcommerce.Web.Tests.Areas.Admin.Controllers;

public class PaymentControllerTests
{
    [Fact]
    public async Task Index_WhenPaymentsExits_ReturnViewWithPayments()
    {
        // Arrange
        var mockPaymentService = new Mock<IPaymentService>();
        mockPaymentService.Setup(s => s.GetAllAsync())
            .ReturnsAsync(new List<Payment>
            {
            new Payment { Id = 1, OrderId = 1, Amount = 10.10m, Method = "11", Status = "Processing", TransactionReference = "123" },
            new Payment { Id = 2, OrderId = 2, Amount = 20.10m, Method = "11", Status = "Processing", TransactionReference = "123" }
            });
        var controller = new PaymentController(mockPaymentService.Object);

        // Act
        var result = await controller.Index();

        //Assert
        var view = Assert.IsType<ViewResult>(result);
        var payments = Assert.IsType<List<Payment>>(view.Model);
        Assert.Equal(2, payments.Count());
    }

    [Fact]
    public async Task Edit_WhenPaymentIdExits_ReturnViewWithPayment()
    {
        // Arrange
        var mockPaymentService = new Mock<IPaymentService>();

        mockPaymentService.Setup(s => s.GetByIdAsync(1))
        .ReturnsAsync(
            new Payment { 
                Id = 1,
                OrderId = 1,
                Amount = 10.10m,
                Method = "11",
                Status = "Processing", 
                TransactionReference = "123" 
                }
        );
        int id = 1;

        var controller = new PaymentController(mockPaymentService.Object);

        // Act
        var result = await controller.Edit(id);

        // Assert
        var view = Assert.IsType<ViewResult>(result);
        var payment = Assert.IsType<Payment>(view.Model);
        Assert.Equal(id, payment.Id);
    }

    [Fact]
    public async Task EditPost_WhenModelIsValid_ReturnRedirectToActionResult()
    {
        // Arrange
        var mockPaymentService = new Mock<IPaymentService>();
        mockPaymentService.Setup(s => s.UpdateAsync(It.IsAny<Payment>()));
        var controller = new PaymentController(mockPaymentService.Object);

        // Act
        Payment model = new Payment { 
            Id = 1,
            OrderId = 1,
            Amount = 10.10m,
            Method = "11", 
            Status = "Processing",
            TransactionReference = "123" 
        };

        var result = await controller.Edit(model);
        // Assert
        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public async Task Delete_WhenOrderIdExits_ReturnViewWithPayment()
    {
        // Arrange
        var mockPaymentService = new Mock<IPaymentService>();

        mockPaymentService.Setup(s => s.GetByIdAsync(1))
        .ReturnsAsync(
            new Payment { 
                Id = 1, 
                OrderId = 1, 
                Amount = 10.10m, 
                Method = "11", 
                Status = "Processing", 
                TransactionReference = "123" 
                }
        );
        int id = 1;

        var controller = new PaymentController(mockPaymentService.Object);

        // Act
        var result = await controller.Delete(id);
        // Assert
        var view = Assert.IsType<ViewResult>(result);
        var payment = Assert.IsType<Payment>(view.Model);
        Assert.Equal(id, payment.Id);
    }

    [Fact]
    public async Task DeleteConfirmed_WhenPaymentIdExits_ReturnRedirectToActionResult()
    {
        // Arrange
        var mockPaymentService = new Mock<IPaymentService>();
        mockPaymentService.Setup(s => s.RemoveAsync(1));
        var controller = new PaymentController(mockPaymentService.Object);
        var id = 1;

        // Act
        var result = await controller.DeleteConfirmed(id);

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
    }
}