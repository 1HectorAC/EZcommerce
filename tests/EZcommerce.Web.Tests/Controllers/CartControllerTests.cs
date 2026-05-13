
using EZcommerce.Web.Controllers;
using EZcommerce.Web.Models.Session;
using EZcommerce.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EZcommerce.Web.Tests.Controllers;

public class CartControllerTests
{

    [Fact]
    public async Task Index_WithCart_ReturnViewWithCartItemList()
    {
        // Arrange
        var mockCartService = new Mock<ICartService>();
        mockCartService.Setup(s => s.GetCart())
            .Returns(new List<CartItem> {
                new CartItem { Name="A", PriceSnapshot=10.10m, ProductId=1, Quantity=1}
            });

        var controller = new CartController(mockCartService.Object);
        // Act
        var result = controller.Index();

        // Assert
        var view = Assert.IsType<ViewResult>(result);
        var cartItems = Assert.IsType<List<CartItem>>(view.Model);
    }

    [Fact]
    public async Task Add_WithValidCartItem_ReturnView()
    {
        // Arrange
        var mockCartService = new Mock<ICartService>();
        mockCartService.Setup(s => s.AddToCart(It.IsAny<CartItem>()));

        var controller = new CartController(mockCartService.Object);
        var cartItem = new CartItem { Name = "B", PriceSnapshot = 10.10m, ProductId = 1, Quantity = 1 };

        // Act
        var result = controller.Add(cartItem);

        // Assert
        var view = Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task LowerQuantity_WithValidProductId_ReturnView()
    {
        // Arrange
        var mockCartService = new Mock<ICartService>();
        mockCartService.Setup(s => s.DecrementCartItemQuantity(1));
        var controller = new CartController(mockCartService.Object);

        // Act
        var result = controller.LowerQuantity(1);

        // Assert
        var view = Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Remove_WithValidId_ReturnRedirectToViewResult()
    {
        // Arrange
        var mockCartService = new Mock<ICartService>();
        mockCartService.Setup(s => s.RemoveFromCart(1));
        var controller = new CartController(mockCartService.Object);

        // Act
        var result = controller.Remove(1);

        // Assert
        var view = Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public async Task ClearCart_WithNothing_ReturnRedirectToViewResult()
    {
        // Arrange
        var mockCartService = new Mock<ICartService>();
        mockCartService.Setup(s => s.ClearCart());
        var controller = new CartController(mockCartService.Object);

        // Act
        var result = controller.ClearCart();

        // Assert
        var view = Assert.IsType<RedirectToActionResult>(result);
    }

}