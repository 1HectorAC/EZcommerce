
using EZcommerce.Web.Data;
using EZcommerce.Web.Models;
using EZcommerce.Web.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace EZcommerce.Web.Tests.Repositories;

public class GenericRepositoryTests
{
    private GenericRepository<T> CreateRepository<T>(out EZcommerceDbContext context)
    where T : class
    {
        var options = new DbContextOptionsBuilder<EZcommerceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new EZcommerceDbContext(options);
        return new GenericRepository<T>(context);
    }

    [Fact]
    public void Query_WithMultipleProducts_ReturnsQueryable()
    {
        // Arrange
        var repo = CreateRepository<Product>(out var context);

        context.Products.AddRange(
            new Product { Id = 1, Name = "A", Description = "a a a", Price = 10.10m, CategoryId = 1, Created_at = new DateTime(2026, 1, 1) },
            new Product { Id = 2, Name = "B", Description = "a a a", Price = 10.10m, CategoryId = 1, Created_at = new DateTime(2026, 1, 1) }
        );
        context.SaveChanges();

        // Act
        var query = repo.Query().Where(p => p.Id > 1).ToList();

        // Assert
        Assert.Single(query);
        Assert.Equal("B", query[0].Name);
    }

    [Fact]
    public async Task GetByIdAsync_WithExistingId_ReturnMatchingProduct()
    {
        // Arrange
        var repo = CreateRepository<Product>(out var context);
        var product = new Product { Id = 10, Name = "A", Description = "a a a", Price = 10.10m, CategoryId = 1, Created_at = new DateTime(2026, 1, 1) };

        context.Products.Add(product);
        await context.SaveChangesAsync();

        // Act
        var result = await repo.GetByIdAsync(10);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("A", result.Name);
    }

    [Fact]
    public async Task AddAsync_WithProducts_AddProductsToDatabase()
    {
        // Arrange
        var repo = CreateRepository<Product>(out var context);
        var product = new Product { Id = 10, Name = "A", Description = "a a a", Price = 10.10m, CategoryId = 1, Created_at = new DateTime(2026, 1, 1) };

        // Act
        await repo.AddAsync(product);
        await repo.SaveChangesAsync();

        // Assert
        Assert.Equal(1, context.Products.Count());
    }

    [Fact]
    public async Task Remove_WithProducts_RemoveProductFromDatabase()
    {
        // Arrange
        var repo = CreateRepository<Product>(out var context);
        var product = new Product { Id = 10, Name = "A", Description = "a a a", Price = 10.10m, CategoryId = 1, Created_at = new DateTime(2026, 1, 1) };

        context.Products.Add(product);
        await context.SaveChangesAsync();

        // Act
        repo.Remove(product);
        await repo.SaveChangesAsync();

        // Assert
        Assert.Empty(context.Products);
    }

    [Fact]
    public async Task SaveChangesAsync_WithPandingChanges_PersistsDataToDatabase()
    {
        // Arrange
        var repo = CreateRepository<Product>(out var context);
        var product = new Product { Id = 10, Name = "A", Description = "a a a", Price = 10.10m, CategoryId = 1, Created_at = new DateTime(2026, 1, 1) };

        // Act
        await repo.AddAsync(product);
        await repo.SaveChangesAsync();

        // Assert
        var saved = context.Products.FirstOrDefault(p => p.Id == 10);
        Assert.NotNull(saved);
    }

}