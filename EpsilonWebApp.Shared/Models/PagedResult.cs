namespace EpsilonWebApp.Shared.Models
{
    /// <summary>
    /// Represents a paginated set of items.
    /// </summary>
    /// <typeparam name="T">The type of items in the result.</typeparam>
    public class PagedResult<T>
    {
        /// <summary>Gets or sets the list of items for the current page.</summary>
        public List<T> Items { get; set; } = new List<T>();
        /// <summary>Gets or sets the total number of items across all pages.</summary>
        public int TotalCount { get; set; }
        /// <summary>Gets or sets the current page number.</summary>
        public int Page { get; set; }
        /// <summary>Gets or sets the number of items per page.</summary>
        public int PageSize { get; set; }
        /// <summary>Gets the total number of pages.</summary>
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
