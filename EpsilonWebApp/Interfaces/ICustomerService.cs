using EpsilonWebApp.Shared.Models;

namespace EpsilonWebApp.Interfaces
{
    /// <summary>
    /// Service interface for business logic related to customers.
    /// </summary>
    public interface ICustomerService
    {
        /// <summary>Gets a paginated list of customers.</summary>
        Task<PagedResult<Customer>> GetCustomersAsync(int page, int pageSize, string? sortBy, bool descending);
        /// <summary>Gets a customer by their unique identifier.</summary>
        Task<Customer?> GetCustomerByIdAsync(Guid id);
        /// <summary>Creates a new customer.</summary>
        Task<Customer> CreateCustomerAsync(Customer customer);
        /// <summary>Updates an existing customer.</summary>
        Task<bool> UpdateCustomerAsync(Guid id, Customer customer);
        /// <summary>Deletes a customer.</summary>
        Task<bool> DeleteCustomerAsync(Guid id);
        /// <summary>Proxies a search request to the OpenStreetMap API.</summary>
        Task<string> SearchAddressAsync(string query);
    }
}
