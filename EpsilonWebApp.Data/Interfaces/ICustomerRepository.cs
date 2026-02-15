using EpsilonWebApp.Shared.Models;

namespace EpsilonWebApp.Data.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<PagedResult<Customer>> GetPagedAsync(int page, int pageSize, string? sortBy, bool descending);
    }
}
