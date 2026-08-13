using DigitalBanking.Application.Features.Customers.Commands.UpdateCustomer;
using DigitalBanking.Application.Interfaces.Common;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Domain.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace DigitalBanking.Application.Tests.Features.Customers.UpdateCustomer
{
    public class UpdateCustomerCommandHandlerTest
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly UpdateCustomerCommandHandler _handler;

        public UpdateCustomerCommandHandlerTest()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new UpdateCustomerCommandHandler(_mockCustomerRepository.Object, _mockUnitOfWork.Object,
                NullLogger<UpdateCustomerCommandHandler>.Instance);
        }

        [Fact]
        public async Task Should_Update_Customer_Data()
        {
            // Arrange
            var utcNow = new DateTime(2026, 12, 1, 12, 0, 0, DateTimeKind.Utc);
            var customer = Customer.Create("fname", "lname", "email@gmail.com", "phone", "password-hash");

            _mockCustomerRepository.Setup(x => x.GetByIdUpdateAsync(customer.Id, It.IsAny<CancellationToken>())).ReturnsAsync(customer);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var command = new UpdateCustomerCommand()
            {
                CustomerId = customer.Id,
                FirstName = "newFirstName",
                LastName = "newLastName",
                Email = "newEmail@gmail.com",
                Phone = "1234567890"
            };

            var result = await _handler.Handle(command, CancellationToken.None);
            result.Should().NotBeNull();
            result.FirstName.Should().Be(command.FirstName);
            result.LastName.Should().Be(command.LastName);
            result.Email.Should().Be("newemail@gmail.com");
            result.PhoneNumber.Should().Be(command.Phone);

            _mockCustomerRepository.Verify(x => x.GetByIdUpdateAsync(customer.Id, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_Error_When_Customer_Not_Found()
        {
            var customerId = Guid.NewGuid();
            var utcNow = new DateTime(2026, 12, 1, 12, 0, 0, DateTimeKind.Utc);

            _mockCustomerRepository.Setup(x => x.GetByIdUpdateAsync(customerId, It.IsAny<CancellationToken>())).ReturnsAsync((Customer?)null);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

            var command = new UpdateCustomerCommand()
            {
                CustomerId = customerId,
                FirstName = "newFirstName",
                LastName = "newLastName",
                Email = "newEmail@gmail.com",
                Phone = "1234567890"
            };

            await Assert.ThrowsAsync<CustomerNotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _mockCustomerRepository.Verify(x => x.GetByIdUpdateAsync(customerId, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Should_Throw_Error_When_Duplicate_Email()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var utcNow = new DateTime(2026, 12, 1, 12, 0, 0, DateTimeKind.Utc);
            var customer = Customer.Create("fname", "lname", "email@gmail.com", "phone", "password-hash");

            _mockCustomerRepository.Setup(x => x.GetByIdUpdateAsync(customerId, It.IsAny<CancellationToken>())).ReturnsAsync(customer);
            _mockCustomerRepository.Setup(x => x.CustomerExistsByEmailAsync(customer.Email, customer.Id, It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(0);

            var command = new UpdateCustomerCommand()
            {
                CustomerId = customerId,
                FirstName = "newFirstName",
                LastName = "newLastName",
                Email = "email@gmail.com",
                Phone = "1234567890"
            };

            await Assert.ThrowsAsync<CustomerAlreadyExistsException>(() => _handler.Handle(command, CancellationToken.None));

            _mockCustomerRepository.Verify(x => x.GetByIdUpdateAsync(customerId, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
