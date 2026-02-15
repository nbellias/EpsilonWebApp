using EpsilonWebApp.Interfaces;
using EpsilonWebApp.Shared.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EpsilonWebApp.Controllers
{
    /// <summary>
    /// API Controller for managing customer data.
    /// </summary>
    [Authorize(AuthenticationSchemes = $"{JwtBearerDefaults.AuthenticationScheme},{CookieAuthenticationDefaults.AuthenticationScheme}")]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersController"/> class.
        /// </summary>
        /// <param name="customerService">The customer service.</param>
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Retrieves a paginated list of customers.
        /// </summary>
        /// <param name="page">The page number to retrieve (default is 1).</param>
        /// <param name="pageSize">The number of items per page (default is 10).</param>
        /// <param name="sortBy">The property name to sort by.</param>
        /// <param name="descending">True to sort in descending order.</param>
        /// <returns>A paginated result containing the list of customers.</returns>
        [HttpGet]
        public async Task<ActionResult<PagedResult<Customer>>> GetCustomers(
            int page = 1, 
            int pageSize = 10, 
            string? sortBy = null, 
            bool descending = false)
        {
            var result = await _customerService.GetCustomersAsync(page, pageSize, sortBy, descending);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific customer by their ID.
        /// </summary>
        /// <param name="id">The unique identifier of the customer.</param>
        /// <returns>The requested customer if found; otherwise, NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(Guid id)
        {
            var customer = await _customerService.GetCustomerByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        /// <summary>
        /// Updates an existing customer.
        /// </summary>
        /// <param name="id">The ID of the customer to update.</param>
        /// <param name="customer">The updated customer data.</param>
        /// <returns>NoContent if successful; otherwise, NotFound.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(Guid id, Customer customer)
        {
            var success = await _customerService.UpdateCustomerAsync(id, customer);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="customer">The customer data to create.</param>
        /// <returns>The created customer with a link to its GET endpoint.</returns>
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            var createdCustomer = await _customerService.CreateCustomerAsync(customer);
            return CreatedAtAction("GetCustomer", new { id = createdCustomer.Id }, createdCustomer);
        }

        /// <summary>
        /// Deletes a specific customer.
        /// </summary>
        /// <param name="id">The ID of the customer to delete.</param>
        /// <returns>NoContent if successful; otherwise, NotFound.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(Guid id)
        {
            var success = await _customerService.DeleteCustomerAsync(id);
            if (!success)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Proxies an address search request to the OpenStreetMap Nominatim API.
        /// </summary>
        /// <param name="query">The address search query.</param>
        /// <returns>The raw JSON response from OpenStreetMap.</returns>
        [HttpGet("search-address")]
        public async Task<IActionResult> SearchAddress([FromQuery] string query)
        {
            var json = await _customerService.SearchAddressAsync(query);
            if (string.IsNullOrEmpty(json))
            {
                return BadRequest("Query is required or error calling service");
            }
            return Content(json, "application/json");
        }
    }
}
