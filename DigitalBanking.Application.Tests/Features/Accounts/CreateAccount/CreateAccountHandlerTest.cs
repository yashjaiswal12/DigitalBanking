using DigitalBanking.Application.Features.Accounts.Commands.CreateAccount;
using DigitalBanking.Application.Interfaces.Common;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Application.Interfaces.Services;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace DigitalBanking.Application.Tests.Features.Accounts.CreateAccount
{
    public class CreateAccountHandlerTest
    {
        private readonly CreateAccountCommandHandler _handler;
        private readonly Mock<ICustomerRepository> _mockCustomerRepo;
        private readonly Mock<IAccountRepository> _mockAccountRepo;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IAccountNumberGenerator> _mockAccNumGenerator;
        private readonly Mock<ICurrentUserService> _mockCurrentUserService;

        public CreateAccountHandlerTest()
        {
            _mockCustomerRepo = new Mock<ICustomerRepository>();
            _mockAccountRepo = new Mock<IAccountRepository>();
            _mockAccNumGenerator = new Mock<IAccountNumberGenerator>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCurrentUserService = new Mock<ICurrentUserService>();

            _handler = new CreateAccountCommandHandler(_mockCustomerRepo.Object, _mockAccountRepo.Object, _mockUnitOfWork.Object,
                _mockAccNumGenerator.Object, NullLogger<CreateAccountCommandHandler>.Instance, _mockCurrentUserService.Object);
        }

        [Fact]
        public async Task Should_Create_Account_If_Customer_Exists_And_Active()
        {
            // Arrange
            var customer = Customer.Create("fname", "lname", "email@gmail.com", "0987654321", "tmp-pswd");
            var accountNumber = "123456789012";
            var userId = Guid.NewGuid();

            _mockCustomerRepo.Setup(x => x.GetCustomerByIdAsync(customer.Id, It.IsAny<CancellationToken>())).ReturnsAsync(customer);
            _mockAccNumGenerator.Setup(x => x.GenerateAsync(It.IsAny<CancellationToken>())).ReturnsAsync(accountNumber);
            _mockCurrentUserService.Setup(x => x.UserId).Returns(userId);

            var command = new CreateAccountCommand { CustomerId = customer.Id, Currency = "INR", InitialBalance = 1000, Type = Domain.Enums.AccountType.Savings };

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotEqual(Guid.Empty, result);

            _mockCustomerRepo.Verify(x => x.GetCustomerByIdAsync(customer.Id, It.IsAny<CancellationToken>()), Times.Once);
            _mockAccNumGenerator.Verify(x => x.GenerateAsync(It.IsAny<CancellationToken>()), Times.Once);

            _mockAccountRepo.Verify(x => x.AddAccountAsync(It.Is<Account>(x => x.CustomerId == command.CustomerId &&
                x.AccountNumber == accountNumber && x.Currency == command.Currency), It.IsAny<CancellationToken>()), Times.Once);

            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_When_Customer_Not_Found()
        {
            //Arrange
            var customerId = Guid.NewGuid();
            _mockCustomerRepo.Setup(x => x.GetCustomerByIdAsync(customerId, It.IsAny<CancellationToken>())).ReturnsAsync((Customer?)null);

            var command = new CreateAccountCommand { CustomerId = customerId, Currency = "INR", InitialBalance = 1000, Type = Domain.Enums.AccountType.Savings };

            //Act and Assert
            await Assert.ThrowsAsync<CustomerNotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _mockCustomerRepo.Verify(x => x.GetCustomerByIdAsync(customerId, It.IsAny<CancellationToken>()), Times.Once);
            _mockAccNumGenerator.Verify(x => x.GenerateAsync(It.IsAny<CancellationToken>()), Times.Never);

            _mockAccountRepo.Verify(x => x.AddAccountAsync(It.Is<Account>(x => x.CustomerId == command.CustomerId && 
                x.Currency == command.Currency), It.IsAny<CancellationToken>()), Times.Never);

            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Should_Throw_When_Customer_Is_Inactive()
        {
            //Arrange
            var customer = Customer.Create("fname", "lname", "email@gmail.com", "0987654321", "tmp-pswd");
            _mockCustomerRepo.Setup(x => x.GetCustomerByIdAsync(customer.Id, It.IsAny<CancellationToken>())).ReturnsAsync(customer);

            customer.Deactivate();

            var command = new CreateAccountCommand { CustomerId = customer.Id, Currency = "INR", InitialBalance = 1000, Type = Domain.Enums.AccountType.Savings };

            //Act and Assert
            await Assert.ThrowsAsync<InvalidCustomerException>(() => _handler.Handle(command, CancellationToken.None));

            _mockCustomerRepo.Verify(x => x.GetCustomerByIdAsync(customer.Id, It.IsAny<CancellationToken>()), Times.Once);
            _mockAccNumGenerator.Verify(x => x.GenerateAsync(It.IsAny<CancellationToken>()), Times.Never);

            _mockAccountRepo.Verify(x => x.AddAccountAsync(It.Is<Account>(x => x.CustomerId == command.CustomerId &&
                x.Currency == command.Currency), It.IsAny<CancellationToken>()), Times.Never);

            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Should_Throw_When_Validation_Fails()
        {
            // Arrange
            var customer = Customer.Create("fname", "lname", "email@gmail.com", "0987654321", "tmp-pswd");
            var accountNumber = "123456789012";

            _mockCustomerRepo.Setup(x => x.GetCustomerByIdAsync(customer.Id, It.IsAny<CancellationToken>())).ReturnsAsync(customer);
            _mockAccNumGenerator.Setup(x => x.GenerateAsync(It.IsAny<CancellationToken>())).ReturnsAsync(accountNumber);
            _mockCurrentUserService.Setup(x => x.UserId).Returns(Guid.Empty);

            var command = new CreateAccountCommand { CustomerId = customer.Id, Currency = "INR", InitialBalance = 1000, Type = Domain.Enums.AccountType.Savings };

            //Act and Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));

            _mockCustomerRepo.Verify(x => x.GetCustomerByIdAsync(customer.Id, It.IsAny<CancellationToken>()), Times.Once);
            _mockAccNumGenerator.Verify(x => x.GenerateAsync(It.IsAny<CancellationToken>()), Times.Once);

            _mockAccountRepo.Verify(x => x.AddAccountAsync(It.Is<Account>(x => x.CustomerId == command.CustomerId &&
                x.AccountNumber == accountNumber && x.Currency == command.Currency), It.IsAny<CancellationToken>()), Times.Never);

            _mockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
