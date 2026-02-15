using System.Net.Http.Json;
using EpsilonWebApp.Shared.Models;

namespace EpsilonWebApp.Client.Services
{
    /// <summary>
    /// Implementation of the client-side customer service.
    /// </summary>
    public class CustomerServiceClient : ICustomerServiceClient
    {
        private readonly HttpClient _http;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerServiceClient"/> class.
        /// </summary>
        /// <param name="http">The HTTP client.</param>
        public CustomerServiceClient(HttpClient http)
        {
            _http = http;
        }

        /// <inheritdoc/>
        public async Task<PagedResult<Customer>> GetCustomersAsync(int page = 1, int pageSize = 10, string? sortBy = null, bool descending = false)
        {
            var url = $"api/customers?page={page}&pageSize={pageSize}";
            if (!string.IsNullOrEmpty(sortBy))
            {
                url += $"&sortBy={sortBy}&descending={descending}";
            }
            return await _http.GetFromJsonAsync<PagedResult<Customer>>(url) ?? new PagedResult<Customer>();
        }

        /// <inheritdoc/>
        public async Task<Customer?> GetCustomerAsync(Guid id)
        {
            return await _http.GetFromJsonAsync<Customer>($"api/customers/{id}");
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public async Task DeleteCustomerAsync(Guid id)
        {
            await _http.DeleteAsync($"api/customers/{id}");
        }

        /// <inheritdoc/>
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
