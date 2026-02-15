using EpsilonWebApp.Data.Interfaces;
using EpsilonWebApp.Interfaces;
using EpsilonWebApp.Shared.Models;

namespace EpsilonWebApp.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<Customer>> GetCustomersAsync(int page, int pageSize, string? sortBy, bool descending)
        {
            return await _unitOfWork.Customers.GetPagedAsync(page, pageSize, sortBy, descending);
        }

        public async Task<Customer?> GetCustomerByIdAsync(Guid id)
        {
            return await _unitOfWork.Customers.GetByIdAsync(id);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            if (customer.Id == Guid.Empty)
            {
                customer.Id = Guid.NewGuid();
            }

            await _unitOfWork.Customers.AddAsync(customer);
            await _unitOfWork.CompleteAsync();
            return customer;
        }

        public async Task<bool> UpdateCustomerAsync(Guid id, Customer customer)
        {
            if (id != customer.Id) return false;

            var existing = await _unitOfWork.Customers.GetByIdAsync(id);
            if (existing == null) return false;

            _unitOfWork.Customers.Update(customer);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<bool> DeleteCustomerAsync(Guid id)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(id);
            if (customer == null) return false;

            _unitOfWork.Customers.Remove(customer);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<string> SearchAddressAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return string.Empty;

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "EpsilonWebApp-Challenge");

            var url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(query)}&format=json&addressdetails=1&limit=5";
            
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            return string.Empty;
        }
    }
}
