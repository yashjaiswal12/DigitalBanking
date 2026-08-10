using DigitalBanking.Application.Features.Customers.Commands.DeactivateCustomer;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace DigitalBanking.Application.Tests.Features.Customers.DeactivateCustomer
{
    public class DeactivateCustomerCommandHandlerTest
    {
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IRefreshTokenRepository> _mockRefreshTokenRepository;
        private readonly DeactivateCustomerCommandHandler _handler;

        public DeactivateCustomerCommandHandlerTest()
        {
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockRefreshTokenRepository = new Mock<IRefreshTokenRepository>();
            _handler = new DeactivateCustomerCommandHandler(_mockCustomerRepository.Object, _mockUnitOfWork.Object,
                NullLogger<DeactivateCustomerCommandHandler>.Instance, _mockRefreshTokenRepository.Object);
        }

        [Fact]
        public async Task Should_Return_True_If_Customer_Deleted()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = Customer.Create("fname", "lname", "email@gmail.com", "0000999909", "hash-pass");
            var refreshToken = RefreshToken.Create(customerId, "tokenValue", DateTime.UtcNow, DateTime.UtcNow);

            _mockCustomerRepository.Setup(x => x.GetByIdUpdateAsync(customerId, It.IsAny<CancellationToken>())).ReturnsAsync(customer);
            _mockUnitOfWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _mockRefreshTokenRepository.Setup(x => x.GetRefreshTokenByCustomerIdAsync(customerId, It.IsAny<CancellationToken>())).ReturnsAsync(refreshToken);

            var command = new DeactivateCustomerCommand { CustomerId = customerId };
            
            //Act
            await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.False(customer.IsActive);
            Assert.True(customer.IsDeleted);

            _mockCustomerRepository.Verify(x => x.GetByIdUpdateAsync(customerId, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_Error_When_Not_Found()
        {
            var customerId = Guid.NewGuid();

            _mockCustomerRepository.Setup(x => x.GetByIdUpdateAsync(customerId, It.IsAny<CancellationToken>())).ReturnsAsync((Customer?) null);
            
            var command = new DeactivateCustomerCommand { CustomerId = customerId };

            await Assert.ThrowsAsync<CustomerNotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _mockCustomerRepository.Verify(x => x.GetByIdUpdateAsync(customerId, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
