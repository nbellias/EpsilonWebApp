using System.Net.Http.Json;
using EpsilonWebApp.Shared.Models;

namespace EpsilonWebApp.Client.Services
{
    public class CustomerServiceClient : ICustomerServiceClient
    {
        private readonly HttpClient _http;

        public CustomerServiceClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<PagedResult<Customer>> GetCustomersAsync(int page = 1, int pageSize = 10, string? sortBy = null, bool descending = false)
        {
            var url = $"api/customers?page={page}&pageSize={pageSize}";
            if (!string.IsNullOrEmpty(sortBy))
            {
                url += $"&sortBy={sortBy}&descending={descending}";
            }
            return await _http.GetFromJsonAsync<PagedResult<Customer>>(url) ?? new PagedResult<Customer>();
        }

        public async Task<Customer?> GetCustomerAsync(Guid id)
        {
            return await _http.GetFromJsonAsync<Customer>($"api/customers/{id}");
        }

        public async Task SaveCustomerAsync(Customer customer)
        {
            if (customer.Id == Guid.Empty)
            {
                await _http.PostAsJsonAsync("api/customers", customer);
            }
            else
            {
                await _http.PutAsJsonAsync($"api/customers/{customer.Id}", customer);
            }
        }

        public async Task DeleteCustomerAsync(Guid id)
        {
            await _http.DeleteAsync($"api/customers/{id}");
        }

        public async Task<List<OsmSearchResult>> SearchAddressAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return new List<OsmSearchResult>();
            
            var response = await _http.GetAsync($"api/customers/search-address?query={Uri.EscapeDataString(query)}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<OsmSearchResult>>() ?? new List<OsmSearchResult>();
            }
            return new List<OsmSearchResult>();
        }
    }
}
