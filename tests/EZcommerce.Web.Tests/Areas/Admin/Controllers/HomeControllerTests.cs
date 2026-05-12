
using EZcommerce.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EZcommerce.Web.Tests.Areas.Admin.Controllers;

public class HomeControllerTests
{
    [Fact]
    public void Index_WhenNothing_ReturnView()
    {
        // Arrange

        var controller = new HomeController();

        // Act
        var result = controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
    }
}