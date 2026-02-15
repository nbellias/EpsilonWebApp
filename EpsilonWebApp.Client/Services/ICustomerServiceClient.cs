using EpsilonWebApp.Shared.Models;

namespace EpsilonWebApp.Client.Services
{
    /// <summary>
    /// Client-side service interface for interacting with the Customers API.
    /// </summary>
    public interface ICustomerServiceClient
    {
        /// <summary>Calls the API to get a paginated list of customers.</summary>
        Task<PagedResult<Customer>> GetCustomersAsync(int page = 1, int pageSize = 10, string? sortBy = null, bool descending = false);
        /// <summary>Calls the API to get a specific customer.</summary>
        Task<Customer?> GetCustomerAsync(Guid id);
        /// <summary>Calls the API to create or update a customer.</summary>
        Task SaveCustomerAsync(Customer customer);
        /// <summary>Calls the API to delete a customer.</summary>
        Task DeleteCustomerAsync(Guid id);
        /// <summary>Calls the server-side proxy to search for addresses.</summary>
        Task<List<OsmSearchResult>> SearchAddressAsync(string query);
    }
}
