
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

    [Fact]
    public async Task Create_WhenCategoriesExits_ReturnView()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var mockEZcommerceService = new Mock<IEZcommerceService>();
        mockEZcommerceService.Setup(s => s.CategoryGetAllAsync())
            .ReturnsAsync(new List<Category>
            {
                new Category { Id = 1, Name = "A" },
                new Category { Id = 2, Name = "B" },

            });
        var controller = new ProductController(mockEZcommerceService.Object, mockProductService.Object);

        // Act
        var result = await controller.Create();
        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task CreatePost_WhenPassedInModelIsValid_ReturnRedirectToActionResult()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var mockEZcommerceService = new Mock<IEZcommerceService>();
        mockProductService.Setup(s => s.AddWithInventoryAsync(It.IsAny<ProductCreateViewModel>()));
        var controller = new ProductController(mockEZcommerceService.Object, mockProductService.Object);

        // Act
        ProductCreateViewModel model = new ProductCreateViewModel
        {
            Id = 1,
            Name = "C",
            Price = 10.00m,
            CategoryId = 1,
            InventoryQuantity = 1
        };
        var result = await controller.Create(model);
        // Assert
        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public async Task Edit_WhenProductIdExits_ReturnViewWithProduct()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var mockEZcommerceService = new Mock<IEZcommerceService>();
        mockEZcommerceService.Setup(s => s.CategoryGetAllAsync())
            .ReturnsAsync(new List<Category>
            {
                new Category { Id = 1, Name = "A" },
                new Category { Id = 2, Name = "B" },
            });
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
                Inventory = new Inventory { Id = 1, ProductId = 1, Quantity = 4 }
            }
        );
        int id = 1;

        var controller = new ProductController(mockEZcommerceService.Object, mockProductService.Object);

        // Act
        var result = await controller.Edit(id);
        // Assert
        var view = Assert.IsType<ViewResult>(result);
        var product = Assert.IsType<ProductCreateViewModel>(view.Model);
        Assert.Equal(id, product.Id);
    }

    [Fact]
    public async Task EditPost_WhenPassedInModelIsValid_ReturnRedirectToActionResult()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var mockEZcommerceService = new Mock<IEZcommerceService>();
        mockProductService.Setup(s => s.UpdateWithInventoryAsync(It.IsAny<ProductCreateViewModel>()));
        var controller = new ProductController(mockEZcommerceService.Object, mockProductService.Object);

        // Act
        ProductCreateViewModel model = new ProductCreateViewModel
        {
            Id = 1,
            Name = "C",
            Price = 10.00m,
            CategoryId = 1,
            InventoryQuantity = 1
        };
        var result = await controller.Edit(model);
        // Assert
        Assert.IsType<RedirectToActionResult>(result);
    }

    [Fact]
    public async Task Delete_WhenProductIdExits_ReturnViewWithProduct()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var mockEZcommerceService = new Mock<IEZcommerceService>();

        mockProductService.Setup(s => s.GetByIdWithInventoryAndCategoryAsync(1))
        .ReturnsAsync(
            new Product { Id = 1, Name = "A", Description = "a a a", Price = 10.10m, CategoryId = 1, Created_at = new DateTime(2026, 1, 1) }
        );
        int id = 1;

        var controller = new ProductController(mockEZcommerceService.Object, mockProductService.Object);

        // Act
        var result = await controller.Delete(id);
        // Assert
        var view = Assert.IsType<ViewResult>(result);
        var product = Assert.IsType<Product>(view.Model);
        Assert.Equal(id, product.Id);
    }

    [Fact]
    public async Task DeleteConfirmed_WhenPassedInIdIsValid_ReturnRedirectToActionResult()
    {
        // Arrange
        var mockProductService = new Mock<IProductService>();
        var mockEZcommerceService = new Mock<IEZcommerceService>();
        mockProductService.Setup(s => s.RemoveAsync(1));
        var controller = new ProductController(mockEZcommerceService.Object, mockProductService.Object);
        var id = 1;
        // Act
        var result = await controller.DeleteConfirmed(id);

        // Assert
        Assert.IsType<RedirectToActionResult>(result);
    }
}