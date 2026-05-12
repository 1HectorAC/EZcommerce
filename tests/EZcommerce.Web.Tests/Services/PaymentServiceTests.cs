
using EZcommerce.Web.Models;
using EZcommerce.Web.Services.Implementations;
using EZcommerce.Web.Tests.Helpers;

namespace EZcommerce.Web.Tests.Services;

public class PaymentServiceTests
{
    [Fact]
    public async Task GetAllAsync_WithNoPayments_ReturnEmptyList()
    {
        //Arrange
        var repo = RepositoryHelper.Create<Payment>(out var context);
        var service = new PaymentService(repo);

        // Act
        var payments = await service.GetAllAsync();

        Assert.Empty(payments);
    }

    [Fact]
    public async Task GetAllAsync_WithPayments_ReturnPaymentList()
    {
        //Arrange
        var repo = RepositoryHelper.Create<Payment>(out var context);
        var service = new PaymentService(repo);

        context.Payments.AddRange(
            new Payment { Id = 1, OrderId = 1, Amount = 10.10m, Method = "11", Status = "Processing", TransactionReference = "123" },
            new Payment { Id = 2, OrderId = 2, Amount = 20.10m, Method = "11", Status = "Processing", TransactionReference = "123" }

        );

        context.SaveChanges();

        // Act
        var payments = await service.GetAllAsync();

        // Assert
        Assert.NotEmpty(payments);
    }

    [Fact]
    public async Task GetByIdAsync_WithNoPayments_ReturnNull()
    {
        //Arrange
        var repo = RepositoryHelper.Create<Payment>(out var context);
        var service = new PaymentService(repo);
        var id = 1;

        // Act
        var payment = await service.GetByIdAsync(id);

        // Assert
        Assert.Null(payment);
    }

    [Fact]
    public async Task GetByIdAsync_WithPayments_ReturnPaymentWithMatchingId()
    {
        //Arrange
        var repo = RepositoryHelper.Create<Payment>(out var context);
        var service = new PaymentService(repo);
        var id = 1;

        context.Payments.AddRange(
            new Payment { Id = 1, OrderId = 1, Amount = 10.10m, Method = "11", Status = "Processing", TransactionReference = "123" },
            new Payment { Id = 2, OrderId = 2, Amount = 20.10m, Method = "11", Status = "Processing", TransactionReference = "123" }
        );
        context.SaveChanges();

        // Act
        var payment = await service.GetByIdAsync(id);

        // Assert
        Assert.NotNull(payment);
        Assert.Equal(id, payment.Id);
    }

    [Fact]
    public async Task UpdateAsync_WithPayments_UpdatePaymentFromDatabase()
    {
        //Arrange
        var repo = RepositoryHelper.Create<Payment>(out var context);
        var service = new PaymentService(repo);

        context.Payments.AddRange(
            new Payment { Id = 1, OrderId = 1, Amount = 10.10m, Method = "11", Status = "Processing", TransactionReference = "123" },
            new Payment { Id = 2, OrderId = 2, Amount = 20.10m, Method = "11", Status = "Processing", TransactionReference = "123" }
        );
        context.SaveChanges();

        var paymentChanges = new Payment
        {
            Id = 1,
            OrderId = 1,
            Amount = 10.10m,
            Method = "11",
            Status = "Paid",
            TransactionReference = "123"
        };


        // Act
        await service.UpdateAsync(paymentChanges);

        var payment = context.Payments.FirstOrDefault(i => i.Id == paymentChanges.Id);
        // Assert
        Assert.NotNull(payment);
        Assert.Equal(payment.Id, paymentChanges.Id);
        Assert.Equal(payment.Status, paymentChanges.Status);
    }

    [Fact]
    public async Task RemoveAsync_WithPayments_RemovePaymentFromDatabase()
    {
        //Arrange
        var repo = RepositoryHelper.Create<Payment>(out var context);
        var service = new PaymentService(repo);

        context.Payments.AddRange(
            new Payment { Id = 1, OrderId = 1, Amount = 10.10m, Method = "11", Status = "Processing", TransactionReference = "123" },
            new Payment { Id = 2, OrderId = 2, Amount = 20.10m, Method = "11", Status = "Processing", TransactionReference = "123" }
        );
        context.SaveChanges();
        var id = 1;

        // Act
        await service.RemoveAsync(id);
        var payment = context.Payments.FirstOrDefault(i => i.Id == id);

        // Assert
        Assert.Null(payment);
    }
}