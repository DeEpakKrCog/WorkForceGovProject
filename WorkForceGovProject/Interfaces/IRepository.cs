using WorkForceGovProject.Models;

namespace WorkForceGovProject.Interfaces
{
    /// <summary>
    /// Generic Repository Interface for all entities
    /// Implements basic CRUD operations
    /// </summary>
    public interface IRepository<T> where T : class
    {
        // Read Operations
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindAsync(System.Linq.Expressions.Expression<System.Func<T, bool>> predicate);
        Task<T> FirstOrDefaultAsync(System.Linq.Expressions.Expression<System.Func<T, bool>> predicate);
        Task<bool> AnyAsync(System.Linq.Expressions.Expression<System.Func<T, bool>> predicate);
        Task<int> CountAsync();
        Task<int> CountAsync(System.Linq.Expressions.Expression<System.Func<T, bool>> predicate);

        // Create Operations
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

        // Update Operations
        Task<T> UpdateAsync(T entity);
        Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entities);

        // Delete Operations
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteAsync(T entity);
        Task<bool> DeleteRangeAsync(IEnumerable<T> entities);

        // Pagination
        Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);

        // Tracking
        void Detach(T entity);
        void DetachAll();
    }
}
