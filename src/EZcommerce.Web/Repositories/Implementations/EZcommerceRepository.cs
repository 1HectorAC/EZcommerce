
using EZcommerce.Web.Data;
using EZcommerce.Web.Models;
using Microsoft.EntityFrameworkCore;
namespace EZcommerce.Web.Repositories.Implementations;

public class EZcommerceRepository: IEZcommerceRepository
{
    private readonly EZcommerceDbContext _context;

    public EZcommerceRepository(EZcommerceDbContext context)
    {
        _context = context;
    }


}