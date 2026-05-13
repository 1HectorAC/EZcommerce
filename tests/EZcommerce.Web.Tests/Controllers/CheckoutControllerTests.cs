
using EZcommerce.Web.Controllers;
using EZcommerce.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EZcommerce.Web.Tests.Controllers;

public class CheckoutControllerTests
{
    
    [Fact]
    public async Task Success_WithNothing_ReturnView()
    {
        // Arrange
        var mockCheckoutService = new Mock<ICheckoutService>();
        var mockEZcommerceService = new Mock<IEZcommerceService>();
        var mockCartService = new Mock<ICartService>();
        mockCartService.Setup(s => s.ClearCart());

        var controller = new CheckoutController(mockCheckoutService.Object, mockEZcommerceService.Object, mockCartService.Object);

        // Act
        var result = controller.Success();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Cancel_WithNothing_ReturnView()
    {
        // Arrange
        var mockCheckoutService = new Mock<ICheckoutService>();
        var mockEZcommerceService = new Mock<IEZcommerceService>();
        var mockCartService = new Mock<ICartService>();

        var controller = new CheckoutController(mockCheckoutService.Object, mockEZcommerceService.Object, mockCartService.Object);

        // Act
        var result = controller.Cancel();

        // Assert
        Assert.IsType<ViewResult>(result);
    }
    
}