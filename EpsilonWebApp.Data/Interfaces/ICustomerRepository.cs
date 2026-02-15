using EpsilonWebApp.Shared.Models;

namespace EpsilonWebApp.Data.Interfaces
{
    /// <summary>
    /// Specialized repository interface for Customer entities.
    /// </summary>
    public interface ICustomerRepository : IRepository<Customer>
    {
        /// <summary>
        /// Retrieves a paginated and sorted list of customers.
        /// </summary>
        /// <param name="page">The current page number.</param>
        /// <param name="pageSize">The number of items per page.</param>
        /// <param name="sortBy">The property name to sort by.</param>
        /// <param name="descending">True for descending order, false for ascending.</param>
        /// <returns>A paginated result of customers.</returns>
        Task<PagedResult<Customer>> GetPagedAsync(int page, int pageSize, string? sortBy, bool descending);
    }
}
