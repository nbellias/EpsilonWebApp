using Bunit;
using Bunit.TestDoubles;
using EpsilonWebApp.Client.Pages;
using EpsilonWebApp.Client.Services;
using EpsilonWebApp.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace EpsilonWebApp.Tests
{
    public class CustomersComponentTests : TestContext
    {
        public CustomersComponentTests()
        {
            // Add basic services needed by the component
            this.AddTestAuthorization().SetAuthorized("testuser");
            
            // Configure JSInterop for QuickGrid
            var module = JSInterop.SetupModule("./_content/Microsoft.AspNetCore.Components.QuickGrid/QuickGrid.razor.js");
            module.SetupModule("init", _ => true);
        }

        [Fact]
        public void CustomersPage_ShouldShowLoading_WhenDataNotLoaded()
        {
            // Arrange
            var mockClient = new Mock<ICustomerServiceClient>();
            // Return a task that never completes to simulate loading
            mockClient.Setup(c => c.GetCustomersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(new TaskCompletionSource<PagedResult<Customer>>().Task);

            Services.AddSingleton(mockClient.Object);

            // Act
            var cut = RenderComponent<Customers>();

            // Assert
            var text = cut.Find("p.text-muted").TextContent;
            Assert.Equal("Φόρτωση δεδομένων...", text);
        }

        [Fact]
        public void CustomersPage_ShouldRenderTable_WhenDataLoaded()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Id = Guid.NewGuid(), CompanyName = "Acme Corp", ContactName = "John Doe", City = "Athens", Country = "Greece" }
            };
            var pagedResult = new PagedResult<Customer>
            {
                Items = customers,
                TotalCount = 1,
                Page = 1,
                PageSize = 10
            };

            var mockClient = new Mock<ICustomerServiceClient>();
            mockClient.Setup(c => c.GetCustomersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(pagedResult);

            Services.AddSingleton(mockClient.Object);

            // Act
            var cut = RenderComponent<Customers>();

            // Assert
            var table = cut.Find("table");
            Assert.NotNull(table);
            Assert.Contains("Acme Corp", cut.Markup);
            Assert.Contains("John Doe", cut.Markup);
        }

        [Fact]
        public async Task ClickingNewCustomer_ShouldOpenModal()
        {
            // Arrange
            var pagedResult = new PagedResult<Customer> { Items = new List<Customer>(), TotalCount = 0, PageSize = 10 };
            var mockClient = new Mock<ICustomerServiceClient>();
            mockClient.Setup(c => c.GetCustomersAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(pagedResult);

            Services.AddSingleton(mockClient.Object);

            var cut = RenderComponent<Customers>();

            // Act
            var newBtn = cut.Find("button.btn-primary");
            await newBtn.ClickAsync(new Microsoft.AspNetCore.Components.Web.MouseEventArgs());

            // Assert
            var modalTitle = cut.Find(".modal-title");
            Assert.Equal("Νέος Πελάτης", modalTitle.TextContent);
        }
    }
}
