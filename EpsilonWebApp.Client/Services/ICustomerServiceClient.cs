using EpsilonWebApp.Shared.Models;

namespace EpsilonWebApp.Client.Services
{
    public interface ICustomerServiceClient
    {
        Task<PagedResult<Customer>> GetCustomersAsync(int page = 1, int pageSize = 10, string? sortBy = null, bool descending = false);
        Task<Customer?> GetCustomerAsync(Guid id);
        Task SaveCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(Guid id);
        Task<List<OsmSearchResult>> SearchAddressAsync(string query);
    }
}
