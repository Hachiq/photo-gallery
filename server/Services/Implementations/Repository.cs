using Core.Contracts;
using Core.Entities;
using Core.Exceptions;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Services.Implementations;

public class Repository(AppDbContext _context) : IRepository
{

    public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> predicate) where T : class
    {
        if (predicate != null)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }
        else
        {
            return await _context.Set<T>().ToListAsync();
        }
    }

    public async Task<T?> GetByIdAsync<T>(long id) where T : class
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<T?> GetByIdAsync<T>(long id, params Expression<Func<T, object>>[] includes) where T : class
    {
        IQueryable<T> query = _context.Set<T>();

        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return await query.FirstOrDefaultAsync(e => EF.Property<long>(e, "Id") == id);
    }

    public async Task<T?> FindAsync<T>(Expression<Func<T, bool>> predicate) where T : class
    {
        return await _context.Set<T>().FirstOrDefaultAsync(predicate);
    }

    public async Task AddAsync<T>(T entity) where T : class
    {
        await _context.Set<T>().AddAsync(entity);
    }

    public void Update<T>(T entity) where T : class
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete<T>(T entity) where T : class
    {
        if (_context.Entry(entity).State == EntityState.Detached)
        {
            _context.Set<T>().Attach(entity);
        }
        _context.Set<T>().Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
