namespace EventManagementAPI.Utilities
{
    /// <summary>
    /// Represents a paginated result wrapper used for API responses.
    /// </summary>
    public class PaginationHelper<T>
    {
        // ======================================================
        // 🔹 PROPERTIES
        // ======================================================
        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        // ======================================================
        // 🔹 CONSTRUCTOR
        // ======================================================
        /// <summary>
        /// Initializes a paginated result.
        /// </summary>
        public PaginationHelper(IEnumerable<T> items, int totalItems, int pageNumber, int pageSize)
        {
            Items = items;
            TotalItems = totalItems;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        // ======================================================
        // 🔹 STATIC CREATOR
        // ======================================================
        /// <summary>
        /// Creates a paginated result from an IQueryable source.
        /// </summary>
        public static PaginationHelper<T> Create(IQueryable<T> source, int pageNumber, int pageSize)
        {
            var totalItems = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PaginationHelper<T>(items, totalItems, pageNumber, pageSize);
        }
    }
}
