
using EZcommerce.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace EZcommerce.Web.Repositories.Implementations;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly EZcommerceDbContext _context;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(EZcommerceDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    public IQueryable<T> Query()
    {
        return _dbSet.AsQueryable();
    }
    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }
    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}