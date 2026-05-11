
using EZcommerce.Web.Controllers;
using EZcommerce.Web.Models;
using EZcommerce.Web.Models.ViewModels;
using EZcommerce.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EZcommerce.Web.Tests.Controllers;

public class HomeControllerTests
{
    [Fact]
    public async Task Index_WhenProductsExits_ReturnViewWithProducts()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var mockCartService = new Mock<ICartService>();
        mockProductService.Setup(s => s.GetAllWithInventoryAndCategoryAsync())
            .ReturnsAsync(new List<Product>
            {
                new Product { Id = 1, Name = "A", Description = "a a a", Price = 10.10m, CategoryId = 1, Created_at = new DateTime(2026, 1, 1) },
                new Product { Id = 2, Name = "B", Description = "a a a", Price = 10.10m, CategoryId = 1, Created_at = new DateTime(2026, 1, 1) }
            });
        var controller = new HomeController(mockProductService.Object, mockCartService.Object);

        // Act
        var result = await controller.Index();

        //Assert
        var view = Assert.IsType<ViewResult>(result);
        var products = Assert.IsType<List<Product>>(view.Model);
        Assert.Equal(2, products.Count());
    }

    [Fact]
    public async Task ProductDetails_WhenProductsExits_ReturnViewWithProduct()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var mockCartService = new Mock<ICartService>();
        mockProductService.Setup(s => s.GetByIdWithInventoryAndCategoryAsync(1))
            .ReturnsAsync(
                new Product
                {
                    Id = 1,
                    Name = "A",
                    Description = "a a a",
                    Price = 10.10m,
                    CategoryId = 1,
                    Created_at = new DateTime(2026, 1, 1),
                    Category = new Category { Id = 1, Name = "Tv" },
                    Inventory = new Inventory { Id = 1, ProductId = 1, Quantity = 5 }
                }
            );
        mockCartService.Setup(s => s.GetCartItemQuantity(1))
            .Returns(2);
        var controller = new HomeController(mockProductService.Object, mockCartService.Object);

        // Act
        var result = await controller.ProductDetails(1);

        // Assert
        var view = Assert.IsType<ViewResult>(result);
        var product = Assert.IsType<ProductDetailsViewModel>(view.Model);
        Assert.Equal(1, product.Id);
    }

    [Fact]
    public void Privacy_WhenNothing_ReturnView()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var mockCartService = new Mock<ICartService>();

        var controller = new HomeController(mockProductService.Object, mockCartService.Object);

        // Act
        var result = controller.Privacy();

        // Assert
        Assert.IsType<ViewResult>(result);
    }
}