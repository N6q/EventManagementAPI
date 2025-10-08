using System.Linq.Expressions;

namespace EventManagementAPI.Repositories.Interfaces
{
    /// <summary>
    /// Defines the base contract for all repository classes.
    /// Provides reusable and optimized CRUD operations
    /// that work with any entity type.
    /// </summary>
    public interface IGenericRepository<T> where T : class
    {
        // ======================================================
        // 🔹 READ OPERATIONS
        // ======================================================

        /// <summary>
        /// Retrieves an entity by its primary key.
        /// </summary>
        Task<T?> GetByIdAsync(object id, bool asNoTracking = true, CancellationToken ct = default);

        /// <summary>
        /// Retrieves all entities with optional filtering, ordering, and eager loading.
        /// </summary>
        Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "",
            bool asNoTracking = true,
            CancellationToken ct = default);

        // ======================================================
        // 🔹 CREATE / UPDATE / DELETE
        // ======================================================

        /// <summary>
        /// Adds a new entity to the database.
        /// </summary>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        Task UpdateAsync(T entity);

        /// <summary>
        /// Deletes an entity (supports soft delete if implemented).
        /// </summary>
        Task DeleteAsync(T entity);

        // ======================================================
        // 🔹 PAGINATION SUPPORT
        // ======================================================

        /// <summary>
        /// Returns paginated data and total record count.
        /// </summary>
        Task<(List<T> Items, int TotalCount)> GetPagedAsync(
            int pageNumber,
            int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            CancellationToken ct = default);

        // ======================================================
        // 🔹 UTILITIES
        // ======================================================

        /// <summary>
        /// Checks if an entity exists based on a given condition.
        /// </summary>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Returns a queryable interface for custom LINQ operations.
        /// </summary>
        IQueryable<T> Query();

        /// <summary>
        /// Saves all pending changes to the database.
        /// </summary>
        Task<int> SaveChangesAsync();

        // ======================================================
        // 🔹 TRANSACTION SUPPORT
        // ======================================================

        /// <summary>
        /// Executes a set of operations within a single database transaction.
        /// </summary>
        Task ExecuteInTransactionAsync(Func<Task> operation);
    }
}
