using EpsilonWebApp.Controllers;
using EpsilonWebApp.Data;
using EpsilonWebApp.Services;
using EpsilonWebApp.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EpsilonWebApp.Tests
{
    public class CustomersControllerTests
    {
        private (AppDbContext context, CustomerService service) GetService()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var context = new AppDbContext(options);
            var unitOfWork = new UnitOfWork(context);
            var service = new CustomerService(unitOfWork);
            return (context, service);
        }

        [Fact]
        public async Task GetCustomers_ReturnsAllCustomers()
        {
            // Arrange
            var (context, service) = GetService();
            context.Customers.Add(new Customer { Id = Guid.NewGuid(), CompanyName = "Test 1" });
            context.Customers.Add(new Customer { Id = Guid.NewGuid(), CompanyName = "Test 2" });
            await context.SaveChangesAsync();

            var controller = new CustomersController(service);

            // Act
            var actionResult = await controller.GetCustomers();
            var result = actionResult.Result as OkObjectResult;
            var pagedResult = result!.Value as PagedResult<Customer>;

            // Assert
            Assert.Equal(2, pagedResult!.Items.Count);
        }

        [Fact]
        public async Task PostCustomer_AddsCustomer()
        {
            // Arrange
            var (context, service) = GetService();
            var controller = new CustomersController(service);
            var newCustomer = new Customer { CompanyName = "New Company" };

            // Act
            await controller.PostCustomer(newCustomer);

            // Assert
            Assert.Equal(1, await context.Customers.CountAsync());
            Assert.Equal("New Company", (await context.Customers.FirstAsync()).CompanyName);
        }
    }
}
