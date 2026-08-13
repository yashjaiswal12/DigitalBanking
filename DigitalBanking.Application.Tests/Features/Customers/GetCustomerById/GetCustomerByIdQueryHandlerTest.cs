using DigitalBanking.Application.Features.Customers.Queries.GetCustomerById;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Domain.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System.ComponentModel.DataAnnotations;

namespace DigitalBanking.Application.Tests.Features.Customers.GetCustomerById
{
    public class GetCustomerByIdQueryHandlerTest
    {
        private readonly Mock<ICustomerRepository> _mockRepository;
        private readonly GetCustomerByIdQueryHandler _handler;

        public GetCustomerByIdQueryHandlerTest()
        {
            _mockRepository = new Mock<ICustomerRepository>();
            _handler = new GetCustomerByIdQueryHandler(_mockRepository.Object, NullLogger<GetCustomerByIdQueryHandler>.Instance);
        }

        [Fact]
        public async Task Should_Return_Customer_When_Exists()
        {
            // Arrange
            var customer = Customer.Create("fname", "lname", "test@gmail.com", "9876509879", "hash-password");
            _mockRepository.Setup(x => x.GetCustomerByIdAsync(customer.Id, It.IsAny<CancellationToken>())).ReturnsAsync(customer);

            var query = new GetCustomerByIdQuery() { Id = customer.Id };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.FirstName.Should().Be("fname");
            result.LastName.Should().Be("lname");
            result.PhoneNumber.Should().Be("9876509879");

            _mockRepository.Verify(x => x.GetCustomerByIdAsync(customer.Id, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_Error_When_Customer_NotFound()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            _mockRepository.Setup(x => x.GetCustomerByIdAsync(customerId, It.IsAny<CancellationToken>())).ReturnsAsync((Customer?)null);
            var query = new GetCustomerByIdQuery { Id = customerId };

            //Act
            await Assert.ThrowsAsync<CustomerNotFoundException>(() => _handler.Handle(query, It.IsAny<CancellationToken>()));
        }
    }
}
