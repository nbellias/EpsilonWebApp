using EpsilonWebApp.Data;
using EpsilonWebApp.Data.Repositories;
using EpsilonWebApp.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EpsilonWebApp.Tests
{
    /// <summary>
    /// Tests specifically focused on scalability and performance optimizations.
    /// </summary>
    public class ScalabilityPerformanceTests
    {
        private AppDbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        /// <summary>
        /// Verifies that the pagination logic handles a larger volume of data correctly.
        /// Even in-memory, this ensures the math for Skip/Take and TotalCount is robust.
        /// </summary>
        [Fact]
        public async Task GetPagedAsync_LargeVolume_ReturnsCorrectTotalCountAndPageSize()
        {
            // Arrange
            using var context = GetContext();
            var repository = new CustomerRepository(context);
            
            const int totalItems = 5000;
            const int pageSize = 50;
            const int targetPage = 10;

            var customers = Enumerable.Range(1, totalItems).Select(i => new Customer
            {
                Id = Guid.NewGuid(),
                CompanyName = $"Company {i:D4}",
                ContactName = $"Contact {i}"
            }).ToList();

            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetPagedAsync(targetPage, pageSize, "companyname", false);

            // Assert
            Assert.Equal(totalItems, result.TotalCount);
            Assert.Equal(pageSize, result.Items.Count);
            Assert.Equal(targetPage, result.Page);
            // Verify we are on the correct "slice" of data
            Assert.Equal("Company 0451", result.Items.First().CompanyName);
        }

        /// <summary>
        /// Verifies that AsNoTracking optimization is actually applied.
        /// This is critical for scaling to millions of users to reduce memory pressure on the server.
        /// </summary>
        [Fact]
        public async Task GetPagedAsync_VerifiesEntitiesAreNotTracked()
        {
            // Arrange
            using var context = GetContext();
            var repository = new CustomerRepository(context);
            
            context.Customers.Add(new Customer { Id = Guid.NewGuid(), CompanyName = "Track Test" });
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetPagedAsync(1, 10, null, false);

            // Assert
            var item = result.Items.First();
            var entry = context.Entry(item);
            
            // In a real scalability scenario, Detached/NoTracking is preferred for read-only lists
            Assert.Equal(EntityState.Detached, entry.State);
        }

        /// <summary>
        /// Tests the efficiency of sorting across large datasets.
        /// While InMemory doesn't use SQL indexes, we verify the sorting logic is correct
        /// for all columns we plan to index in production.
        /// </summary>
        [Theory]
        [InlineData("companyname")]
        [InlineData("contactname")]
        [InlineData("city")]
        [InlineData("country")]
        [InlineData("phone")]
        public async Task GetPagedAsync_SortingLogic_WorksCorrectly(string sortBy)
        {
            // Arrange
            using var context = GetContext();
            var repository = new CustomerRepository(context);
            
            context.Customers.AddRange(
                new Customer { Id = Guid.NewGuid(), CompanyName = "B", ContactName = "Z", City = "Athens", Country = "Greece", Phone = "2" },
                new Customer { Id = Guid.NewGuid(), CompanyName = "A", ContactName = "A", City = "Zante", Country = "Albania", Phone = "1" }
            );
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetPagedAsync(1, 10, sortBy, false);

            // Assert
            // When sorting ascending, 'A' should come before 'B', '1' before '2', etc.
            var first = result.Items.First();
            if (sortBy == "companyname") Assert.Equal("A", first.CompanyName);
            if (sortBy == "contactname") Assert.Equal("A", first.ContactName);
            if (sortBy == "city") Assert.Equal("Athens", first.City);
            if (sortBy == "country") Assert.Equal("Albania", first.Country);
            if (sortBy == "phone") Assert.Equal("1", first.Phone);
        }
    }
}
