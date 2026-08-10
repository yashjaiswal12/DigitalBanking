using DigitalBanking.Application.Features.Customers.SearchCustomers.Queries;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Domain.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace DigitalBanking.Application.Tests.Features.Customers.SearchCustomers
{
    public class SearchCustomerQueryHandlerTests
    {
        private readonly SearchCustomerQueryHandler _handler;
        private readonly Mock<ICustomerRepository> _mockRepository;

        public SearchCustomerQueryHandlerTests()
        {
            _mockRepository = new Mock<ICustomerRepository>();
            _handler = new SearchCustomerQueryHandler(_mockRepository.Object, NullLogger<SearchCustomerQueryHandler>.Instance);
        }

        [Fact]
        public async Task Handle_Should_Return_MatchingCustomers()
        {
            // Arrange 
            var customers = new List<Customer>
            {
                Customer.Create("yash", "jaiswal", "abc@gmail.com", "9876543210", "hashed-password")
            };

            _mockRepository.Setup(x => x.SearchCustomerAsync("yash", true, It.IsAny<CancellationToken>())).ReturnsAsync(customers);

            var query = new SearchCustomerQuery() { SearchTerm = "yash", IsActive = true };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().HaveCount(1);
            result[0].FirstName.Should().Be("yash");
            result[0].LastName.Should().Be("jaiswal");
            result[0].Email.Should().Be("abc@gmail.com");
        }

        [Fact]
        public async Task Should_Throw_Error_When_Empty()
        {
            // Arrange
            var query = new SearchCustomerQuery() { SearchTerm = "Unknown", IsActive = true };

            _mockRepository.Setup(x => x.SearchCustomerAsync(query.SearchTerm, query.IsActive, It.IsAny<CancellationToken>()));

            // Act And

            // Assert
            await Assert.ThrowsAsync<CustomerNotFoundException>(() => _handler.Handle(query, CancellationToken.None) );
        }
    }
}
