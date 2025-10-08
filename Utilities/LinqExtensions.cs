using System.Linq.Expressions;

namespace EventManagementAPI.Utilities
{
    /// <summary>
    /// Provides LINQ extension methods for filtering, sorting, and pagination.
    /// Simplifies query logic in repositories and services.
    /// </summary>
    public static class LinqExtensions
    {
        // ======================================================
        // 🔹 DYNAMIC FILTER
        // ======================================================
        /// <summary>
        /// Applies a filter condition to a query only if the predicate is not null.
        /// </summary>
        public static IQueryable<T> WhereIf<T>(
            this IQueryable<T> query,
            bool condition,
            Expression<Func<T, bool>> predicate)
        {
            return condition ? query.Where(predicate) : query;
        }

        // ======================================================
        // 🔹 DYNAMIC SORTING
        // ======================================================
        /// <summary>
        /// Applies dynamic sorting to a query based on a property name.
        /// </summary>
        public static IQueryable<T> OrderByPropertyName<T>(
            this IQueryable<T> source,
            string? propertyName,
            bool ascending = true)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                return source;

            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            var selector = Expression.Lambda(property, parameter);

            var method = ascending ? "OrderBy" : "OrderByDescending";

            var expression = Expression.Call(
                typeof(Queryable),
                method,
                new Type[] { typeof(T), property.Type },
                source.Expression,
                Expression.Quote(selector));

            return source.Provider.CreateQuery<T>(expression);
        }

        // ======================================================
        // 🔹 PAGINATION
        // ======================================================
        /// <summary>
        /// Applies pagination to an IQueryable collection.
        /// </summary>
        public static IQueryable<T> Paginate<T>(
            this IQueryable<T> query,
            int pageNumber,
            int pageSize)
        {
            if (pageNumber <= 0) pageNumber = 1;
            if (pageSize <= 0) pageSize = 10;

            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
