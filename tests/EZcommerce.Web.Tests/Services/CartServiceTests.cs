
using System.Text;
using EZcommerce.Web.Models.Session;
using EZcommerce.Web.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Moq;

namespace EZcommerce.Web.Tests.Services;

public class CartServiceTests
{
    public static Mock<ISession> CreateSessionMock()
    {
        var store = new Dictionary<string, byte[]>();

        var mock = new Mock<ISession>();

        mock.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
            .Callback<string, byte[]>((key, value) =>
            {
                store[key] = value;
            });

        mock.Setup(s => s.TryGetValue(It.IsAny<string>(), out It.Ref<byte[]>.IsAny))
            .Returns((string key, out byte[] value) =>
            {
                var exists = store.TryGetValue(key, out var stored);
                value = stored;
                return exists;
            });

        mock.Setup(s => s.Remove(It.IsAny<string>()))
            .Callback<string>(key =>
            {
                store.Remove(key);
            });


        return mock;
    }

    [Fact]
    public async Task GetCart_EmptyCart_ReturnEmptyList()
    {
        // Arrange
        var sessionMock = CreateSessionMock();


        var httpContext = new DefaultHttpContext
        {
            Session = sessionMock.Object
        };

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);

        var service = new CartService(accessor.Object);

        // Act
        var cart = service.GetCart();

        // Assert
        Assert.Empty(cart);
    }

    [Fact]
    public void GetCart_WithCartItems_ReturnCartItemsList()
    {
        // Arrange
        var sessionMock = CreateSessionMock();

        var httpContext = new DefaultHttpContext
        {
            Session = sessionMock.Object
        };

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);

        var service = new CartService(accessor.Object);

        service.SaveCart(new List<CartItem>
        {
            new CartItem {ProductId=1, Name="A", PriceSnapshot=10.00m, Quantity=1},
            new CartItem {ProductId=2, Name="B", PriceSnapshot=15.00m, Quantity=1}
        });

        // Act
        var cart = service.GetCart();

        // Assert
        Assert.NotEmpty(cart);
        Assert.Equal(2, cart.Count());
    }

    [Fact]
    public void GetCount_EmptyCart_Return0()
    {
        // Arrange
        var sessionMock = CreateSessionMock();

        var httpContext = new DefaultHttpContext
        {
            Session = sessionMock.Object
        };

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);

        var service = new CartService(accessor.Object);

        // Act
        var count = service.GetCount();

        Assert.Equal(0, count);
    }

    [Fact]
    public void GetCount_SomeCartItems_ReturnCount()
    {
        // Arrange
        var sessionMock = CreateSessionMock();

        var httpContext = new DefaultHttpContext
        {
            Session = sessionMock.Object
        };

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);

        var service = new CartService(accessor.Object);

        service.SaveCart(new List<CartItem>
        {
            new CartItem {ProductId=1, Name="A", PriceSnapshot=10.00m, Quantity=1},
            new CartItem {ProductId=2, Name="B", PriceSnapshot=15.00m, Quantity=1}
        });

        // Act
        var count = service.GetCount();

        Assert.Equal(2, count);
    }

    [Fact]
    public void GetInventoryCount_SomeCartItems_ReturnInventoryCount()
    {
        // Arrange
        var sessionMock = CreateSessionMock();

        var httpContext = new DefaultHttpContext
        {
            Session = sessionMock.Object
        };

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);

        var service = new CartService(accessor.Object);

        service.SaveCart(new List<CartItem>
        {
            new CartItem {ProductId=1, Name="A", PriceSnapshot=10.00m, Quantity=2},
            new CartItem {ProductId=2, Name="B", PriceSnapshot=15.00m, Quantity=1}
        });

        // Act
        var count = service.GetInventoryCount();

        Assert.Equal(3, count);
    }

    [Fact]
    public void GetCartItemQuantity_SomeCartItems_ReturnCartItemQuantity()
    {
        // Arrange
        var sessionMock = CreateSessionMock();

        var httpContext = new DefaultHttpContext
        {
            Session = sessionMock.Object
        };

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);

        var service = new CartService(accessor.Object);

        service.SaveCart(new List<CartItem>
        {
            new CartItem {ProductId=1, Name="A", PriceSnapshot=10.00m, Quantity=2},
            new CartItem {ProductId=2, Name="B", PriceSnapshot=15.00m, Quantity=1}
        });

        // Act
        var quantity = service.GetCartItemQuantity(1);

        Assert.Equal(2, quantity);
    }

    [Fact]
    public void SaveCart_EmptyCart_SetCartItemsToSession()
    {
        // Arrange
        var sessionMock = CreateSessionMock();

        var httpContext = new DefaultHttpContext
        {
            Session = sessionMock.Object
        };

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);

        var service = new CartService(accessor.Object);


        // Act
        service.SaveCart(new List<CartItem>
        {
            new CartItem {ProductId=1, Name="A", PriceSnapshot=10.00m, Quantity=2}
        });

        Assert.Equal(1, service.GetCount());
    }

    [Fact]
    public void AddToCart_WithCartItems_AddCartItemsToSession()
    {
        // Arrange
        var sessionMock = CreateSessionMock();

        var httpContext = new DefaultHttpContext
        {
            Session = sessionMock.Object
        };

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);

        var service = new CartService(accessor.Object);

        service.SaveCart(new List<CartItem>
        {
            new CartItem {ProductId=1, Name="A", PriceSnapshot=10.00m, Quantity=2}
        });

        // Act
        service.AddToCart(new CartItem { ProductId = 2, Name = "A", PriceSnapshot = 10.00m, Quantity = 2 });

        Assert.Equal(2, service.GetCount());
    }

    [Fact]
    public void RemoveFromCart_WithCartItem_RemoveCartItemsFromSession()
    {
        // Arrange
        var sessionMock = CreateSessionMock();

        var httpContext = new DefaultHttpContext
        {
            Session = sessionMock.Object
        };

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);

        var service = new CartService(accessor.Object);

        service.SaveCart(new List<CartItem>
        {
            new CartItem {ProductId=1, Name="A", PriceSnapshot=10.00m, Quantity=2}
        });

        // Act
        service.RemoveFromCart(1);

        Assert.Equal(0, service.GetCount());
    }

    [Fact]
    public void DecrementCartItemQuantity_WithCartItem_LowerQuantityOfCartItemInSession()
    {
        // Arrange
        var sessionMock = CreateSessionMock();

        var httpContext = new DefaultHttpContext
        {
            Session = sessionMock.Object
        };

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);

        var service = new CartService(accessor.Object);

        service.SaveCart(new List<CartItem>
        {
            new CartItem {ProductId=1, Name="A", PriceSnapshot=10.00m, Quantity=5}
        });
        var id = 1;

        // Act
        service.DecrementCartItemQuantity(id);

        Assert.Equal(4, service.GetCartItemQuantity(id));
    }

    [Fact]
    public void ClearCart_WithCartItem_EmptySession()
    {
        // Arrange
        var sessionMock = CreateSessionMock();

        var httpContext = new DefaultHttpContext
        {
            Session = sessionMock.Object
        };

        var accessor = new Mock<IHttpContextAccessor>();
        accessor.Setup(a => a.HttpContext).Returns(httpContext);

        var service = new CartService(accessor.Object);

        service.SaveCart(new List<CartItem>
        {
            new CartItem {ProductId=1, Name="A", PriceSnapshot=10.00m, Quantity=5}
        });

        // Act
        service.ClearCart();

        Assert.Equal(0, service.GetCount());
    }
}