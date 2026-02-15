using EpsilonWebApp.Data.Interfaces;
using EpsilonWebApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EpsilonWebApp.Data.Repositories
{
    /// <summary>
    /// Implementation of the customer repository.
    /// </summary>
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public CustomerRepository(AppDbContext context) : base(context)
        {
        }

        /// <inheritdoc/>
        public async Task<PagedResult<Customer>> GetPagedAsync(int page, int pageSize, string? sortBy, bool descending)
        {
            IQueryable<Customer> query = Context.Set<Customer>();

            // Apply sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                query = sortBy.ToLower() switch
                {
                    "companyname" => descending ? query.OrderByDescending(c => c.CompanyName) : query.OrderBy(c => c.CompanyName),
                    "contactname" => descending ? query.OrderByDescending(c => c.ContactName) : query.OrderBy(c => c.ContactName),
                    "city" => descending ? query.OrderByDescending(c => c.City) : query.OrderBy(c => c.City),
                    "country" => descending ? query.OrderByDescending(c => c.Country) : query.OrderBy(c => c.Country),
                    "phone" => descending ? query.OrderByDescending(c => c.Phone) : query.OrderBy(c => c.Phone),
                    _ => query.OrderBy(c => c.CompanyName)
                };
            }
            else
            {
                query = query.OrderBy(c => c.CompanyName);
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Customer>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}
