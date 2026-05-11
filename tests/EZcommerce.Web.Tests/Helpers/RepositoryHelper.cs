
using EZcommerce.Web.Data;
using EZcommerce.Web.Repositories.Implementations;
using Microsoft.EntityFrameworkCore;

namespace EZcommerce.Web.Tests.Helpers;

public static class RepositoryHelper
{
    public static GenericRepository<T> Create<T>(out EZcommerceDbContext context)
where T : class
    {
        var options = new DbContextOptionsBuilder<EZcommerceDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        context = new EZcommerceDbContext(options);
        return new GenericRepository<T>(context);
    }
}