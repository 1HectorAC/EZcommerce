
using EZcommerce.Areas.Admin.Controllers;
using EZcommerce.Web.Models;
using EZcommerce.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EZcommerce.Web.Tests.Areas.Admin.Controllers;

public class ProductControllerTests
{
    [Fact]
    public async Task Index_WhenProductsExits_ReturnViewWithProducts()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var mockEZcommerceService = new Mock<IEZcommerceService>();
        mockProductService.Setup(s => s.GetAllWithInventoryAndCategoryAsync())
            .ReturnsAsync(new List<Product>
            {
                new Product { Id = 1, Name = "A", Description = "a a a", Price = 10.10m, CategoryId = 1, Created_at = new DateTime(2026, 1, 1) },
                new Product { Id = 2, Name = "B", Description = "a a a", Price = 10.10m, CategoryId = 1, Created_at = new DateTime(2026, 1, 1) }
            });
        var controller = new ProductController(mockEZcommerceService.Object, mockProductService.Object);

        // Act
        var result = await controller.Index();

        //Assert
        var view = Assert.IsType<ViewResult>(result);
        var products = Assert.IsType<List<Product>>(view.Model);
        Assert.Equal(2, products.Count());
    }
}