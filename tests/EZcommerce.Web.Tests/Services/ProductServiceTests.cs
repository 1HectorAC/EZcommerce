
using EZcommerce.Web.Data;
using EZcommerce.Web.Models;
using EZcommerce.Web.Repositories.Implementations;
using EZcommerce.Web.Services.Implementations;
using EZcommerce.Web.Tests.Helpers;
using Microsoft.EntityFrameworkCore;

namespace EZcommerce.Web.Tests.Services;

public class ProductServiceTests
{
    [Fact]
    public async Task GetAllWithInventoryAndCategoryAsync_NoProducts_ReturnEmptyList()
    {
        //Arrange
        var repo = RepositoryHelper.Create<Product>(out var context);
        var service = new ProductService(repo);

        // Act
        var products = await service.GetAllWithInventoryAndCategoryAsync();

        Assert.Empty(products);
    }

    [Fact]
    public async Task GetAllWithInventoryAndCategoryAsync_WithProducts_ReturnProductsList()
    {
        //Arrange
        var repo = RepositoryHelper.Create<Product>(out var context);
        var service = new ProductService(repo);
        context.Categories.Add(new Category{Id = 1, Name="Stuff"});

        context.Products.AddRange(
            new Product { Id = 1, Name = "A", Description = "a a a", Price = 10.10m, CategoryId = 1, Created_at = new DateTime(2026, 1, 1) },
            new Product { Id = 2, Name = "B", Description = "a a a", Price = 10.10m, CategoryId = 1, Created_at = new DateTime(2026, 1, 1) }
        );
        context.Inventories.AddRange(
            new Inventory {Id = 1, ProductId = 1},
            new Inventory {Id = 2, ProductId = 2}

        );
        context.SaveChanges();

        // Act
        var products = await service.GetAllWithInventoryAndCategoryAsync();

        // Assert
        Assert.NotEmpty(products);
    }

    [Fact]
    public async Task GetByIdWithInventoryAndCategoryAsync_NoProducts_ReturnNull()
    {
        //Arrange
        var repo = RepositoryHelper.Create<Product>(out var context);
        var service = new ProductService(repo);
        var id = 1;

        // Act
        var product = await service.GetByIdWithInventoryAndCategoryAsync(id);

        // Assert
        Assert.Null(product);
    }
    [Fact]
    public async Task GetByIdWithInventoryAndCategoryAsync_WithProducts_ReturnProductWithMatchingId()
    {
        //Arrange
        var repo = RepositoryHelper.Create<Product>(out var context);
        var service = new ProductService(repo);
        
        context.Categories.Add(new Category{Id = 1, Name="Stuff"});

        context.Products.AddRange(
            new Product { Id = 1, Name = "A", Description = "a a a", Price = 10.10m, CategoryId = 1, Created_at = new DateTime(2026, 1, 1) },
            new Product { Id = 2, Name = "B", Description = "a a a", Price = 10.10m, CategoryId = 1, Created_at = new DateTime(2026, 1, 1) }
        );
        context.Inventories.AddRange(
            new Inventory {Id = 1, ProductId = 1},
            new Inventory {Id = 2, ProductId = 2}

        );
        context.SaveChanges();
        var id = 1;

        // Act
        var product = await service.GetByIdWithInventoryAndCategoryAsync(id);

        // Assert
        Assert.NotNull(product);
        Assert.Equal(id, product.Id);
    }

    [Fact]
    public async Task AddWithInventoryAsync_NoProducts_AddProductToDatabase()
    {
        //Arrange
        var repo = RepositoryHelper.Create<Product>(out var context);
        var service = new ProductService(repo);
        var product = new ProductCreateViewModel
        {
            Id = 1,
            Name = "tv",
            Description = "a tv",
            Price = 1000.00m,
            CategoryId = 1,
            InventoryQuantity = 5
        };

        // Act
        await service.AddWithInventoryAsync(product);

        // Assert
        Assert.Equal(1, context.Products.Count());
    }


    [Fact]
    public async Task UpdateWithInventoryAsync_WithProducts_UpdateProductFromDatabase()
    {
        //Arrange
        var repo = RepositoryHelper.Create<Product>(out var context);
        var service = new ProductService(repo);
        context.Products.AddRange(
            new Product { Id = 1, Name = "A", Description = "a a a", Price = 10.10m, CategoryId = 1, Created_at = new DateTime(2026, 1, 1) },
            new Product { Id = 2, Name = "B", Description = "a a a", Price = 10.10m, CategoryId = 1, Created_at = new DateTime(2026, 1, 1) }
        );
        context.Inventories.AddRange(
            new Inventory { Id = 1, ProductId = 1, Quantity = 0 },
            new Inventory { Id = 2, ProductId = 2, Quantity = 2 }
        );
        context.SaveChanges();
        var productChanges = new ProductCreateViewModel
        {
            Id = 1,
            Name = "A2",
            Description = "a a a",
            Price = 2000.00m,
            CategoryId = 1,
            InventoryQuantity = 5
        };

        // Act
        await service.UpdateWithInventoryAsync(productChanges);

        var product = context.Products.FirstOrDefault(i => i.Id == productChanges.Id);
        // Assert
        Assert.NotNull(product);
        Assert.Equal(product.Id, productChanges.Id);
        Assert.Equal(product.Name, productChanges.Name);
    }

    [Fact]
    public async Task RemoveAsync_WithProducts_RemoveProductFromDatabase()
    {
        //Arrange
        var repo = RepositoryHelper.Create<Product>(out var context);
        var service = new ProductService(repo);
        context.Products.AddRange(
            new Product { Id = 1, Name = "A", Description = "a a a", Price = 10.10m, CategoryId = 1, Created_at = new DateTime(2026, 1, 1) },
            new Product { Id = 2, Name = "B", Description = "a a a", Price = 10.10m, CategoryId = 1, Created_at = new DateTime(2026, 1, 1) }
        );
        context.SaveChanges();
        var id = 1;

        // Act
        await service.RemoveAsync(id);
        var product = context.Products.FirstOrDefault(i => i.Id == id);

        // Assert
        Assert.Null(product);
    }

}