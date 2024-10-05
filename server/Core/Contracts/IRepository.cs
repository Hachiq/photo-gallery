using System.Linq.Expressions;

namespace Core.Contracts;

public interface IRepository
{
    Task<IEnumerable<T>> GetAllAsync<T>() where T : class;
    Task<T?> GetByIdAsync<T>(Guid id) where T : class;
    Task<T?> FindAsync<T>(Expression<Func<T, bool>> predicate) where T : class;
    Task AddAsync<T>(T entity) where T : class;
    void Update<T>(T entity) where T : class;
    void Delete<T>(T entity) where T : class;
    Task SaveChangesAsync();
}
