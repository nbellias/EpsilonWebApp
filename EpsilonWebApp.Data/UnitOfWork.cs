using EpsilonWebApp.Data.Interfaces;
using EpsilonWebApp.Data.Repositories;

namespace EpsilonWebApp.Data
{
    /// <summary>
    /// Implementation of the Unit of Work pattern.
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private ICustomerRepository? _customers;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public ICustomerRepository Customers => _customers ??= new CustomerRepository(_context);

        /// <inheritdoc/>
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
