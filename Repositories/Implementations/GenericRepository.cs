using EventManagementAPI.Data;
using EventManagementAPI.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace EventManagementAPI.Repositories.Implementations
{
    /// <summary>
    /// Provides a reusable and optimized implementation of common CRUD operations.
    /// Includes pagination, soft delete handling, and transaction safety.
    /// </summary>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDbContext _db;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext db)
        {
            _db = db;
            _dbSet = _db.Set<T>();
        }

        // ======================================================
        // 🔹 GET BY ID
        // ======================================================
        public virtual async Task<T?> GetByIdAsync(object id, bool asNoTracking = true, CancellationToken ct = default)
        {
            var entity = await _dbSet.FindAsync(new object[] { id }, ct);
            if (entity == null) return null;

            if (asNoTracking)
                _db.Entry(entity).State = EntityState.Detached;

            return entity;
        }

        // ======================================================
        // 🔹 GET ALL
        // ======================================================
        public virtual async Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "",
            bool asNoTracking = true,
            CancellationToken ct = default)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(includeProperty);

            if (orderBy != null)
                query = orderBy(query);

            if (asNoTracking)
                query = query.AsNoTracking();

            return await query.ToListAsync(ct);
        }

        // ======================================================
        // 🔹 ADD
        // ======================================================
        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await SaveChangesAsync();
            return entity;
        }

        // ======================================================
        // 🔹 UPDATE
        // ======================================================
        public virtual async Task UpdateAsync(T entity)
        {
            if (_db.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);

            _db.Entry(entity).State = EntityState.Modified;
            await SaveChangesAsync();
        }

        // ======================================================
        // 🔹 DELETE (Supports Soft Delete)
        // ======================================================
        public virtual async Task DeleteAsync(T entity)
        {
            var isSoftDeletable = entity.GetType().GetProperty("IsDeleted") != null;

            if (isSoftDeletable)
            {
                entity.GetType().GetProperty("IsDeleted")!.SetValue(entity, true);
                await UpdateAsync(entity);
            }
            else
            {
                _dbSet.Remove(entity);
                await SaveChangesAsync();
            }
        }

        // ======================================================
        // 🔹 PAGINATION SUPPORT
        // ======================================================
        public virtual async Task<(List<T> Items, int TotalCount)> GetPagedAsync(
            int pageNumber, int pageSize,
            Expression<Func<T, bool>>? filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            CancellationToken ct = default)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
                query = query.Where(filter);

            var totalCount = await query.CountAsync(ct);

            if (orderBy != null)
                query = orderBy(query);

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync(ct);

            return (items, totalCount);
        }

        // ======================================================
        // 🔹 UTILITIES
        // ======================================================
        public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.AnyAsync(predicate);

        public IQueryable<T> Query()
            => _dbSet.AsQueryable();

        public async Task<int> SaveChangesAsync()
            => await _db.SaveChangesAsync();

        // ======================================================
        // 🔹 TRANSACTION SUPPORT
        // ======================================================
        /// <summary>
        /// Executes multiple operations inside a single database transaction.
        /// </summary>
        public async Task ExecuteInTransactionAsync(Func<Task> operation)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                await operation();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
