using EpsilonWebApp.Data.Interfaces;
using EpsilonWebApp.Interfaces;
using EpsilonWebApp.Services;
using EpsilonWebApp.Shared.Models;
using Moq;
using Moq.Protected;
using System.Net;
using Xunit;

namespace EpsilonWebApp.Tests
{
    public class CustomerServiceUnitTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly CustomerService _service;

        public CustomerServiceUnitTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _service = new CustomerService(_mockUnitOfWork.Object, _mockHttpClientFactory.Object);
        }

        [Fact]
        public async Task CreateCustomerAsync_ShouldAssignId_WhenIdIsEmpty()
        {
            // Arrange
            var customer = new Customer { Id = Guid.Empty, CompanyName = "Test" };
            _mockUnitOfWork.Setup(u => u.Customers.AddAsync(It.IsAny<Customer>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _service.CreateCustomerAsync(customer);

            // Assert
            Assert.NotEqual(Guid.Empty, result.Id);
            _mockUnitOfWork.Verify(u => u.Customers.AddAsync(It.IsAny<Customer>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateCustomerAsync_ShouldReturnFalse_WhenIdMismatch()
        {
            // Arrange
            var id = Guid.NewGuid();
            var customer = new Customer { Id = Guid.NewGuid() };

            // Act
            var result = await _service.UpdateCustomerAsync(id, customer);

            // Assert
            Assert.False(result);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ShouldReturnFalse_WhenCustomerNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mockUnitOfWork.Setup(u => u.Customers.GetByIdAsync(id))
                .ReturnsAsync((Customer?)null);

            // Act
            var result = await _service.DeleteCustomerAsync(id);

            // Assert
            Assert.False(result);
            _mockUnitOfWork.Verify(u => u.Customers.Remove(It.IsAny<Customer>()), Times.Never);
        }

        [Fact]
        public async Task SearchAddressAsync_ShouldReturnData_WhenSuccessful()
        {
            // Arrange
            var query = "test";
            var expectedJson = "[{\"display_name\": \"Test Address\"}]";
            
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedJson),
                });

            var httpClient = new HttpClient(handlerMock.Object);
            _mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _service.SearchAddressAsync(query);

            // Assert
            Assert.Equal(expectedJson, result);
        }
    }
}
