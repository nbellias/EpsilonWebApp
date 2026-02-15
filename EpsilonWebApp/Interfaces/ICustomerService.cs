using EpsilonWebApp.Shared.Models;

namespace EpsilonWebApp.Interfaces
{
    public interface ICustomerService
    {
        Task<PagedResult<Customer>> GetCustomersAsync(int page, int pageSize, string? sortBy, bool descending);
        Task<Customer?> GetCustomerByIdAsync(Guid id);
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<bool> UpdateCustomerAsync(Guid id, Customer customer);
        Task<bool> DeleteCustomerAsync(Guid id);
        Task<string> SearchAddressAsync(string query);
    }
}
