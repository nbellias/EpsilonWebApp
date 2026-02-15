using EpsilonWebApp.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace EpsilonWebApp.Data
{
    /// <summary>
    /// The database context for the application.
    /// </summary>
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options for the context.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>Gets or sets the Customers database set.</summary>
        public DbSet<Customer> Customers { get; set; }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Customer>().HasKey(c => c.Id);
        }
    }
}
