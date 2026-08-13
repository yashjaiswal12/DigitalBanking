using DigitalBanking.Application.Features.Accounts.Commands.ActivateAccount;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace DigitalBanking.Application.Tests.Features.Accounts.ActivateAccount
{
    public class ActivateAccountHandlerTest
    {
        private readonly ActivateAccountCommandHandler _handler;
        private readonly Mock<IAccountRepository> _mockRepository;
        private readonly Mock<IUnitOfWork> _mockWork;

        public ActivateAccountHandlerTest()
        {
            _mockRepository = new Mock<IAccountRepository>();
            _mockWork = new Mock<IUnitOfWork>();
            _handler = new ActivateAccountCommandHandler(_mockRepository.Object, _mockWork.Object, NullLogger<ActivateAccountCommandHandler>.Instance);
        }

        [Fact]
        public async Task Should_Activate_Account()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var account = Account.Create("123456789012", customerId, Domain.Enums.AccountType.Savings, "INR", 1000, "test-created-by");

            _mockRepository.Setup(x => x.GetByIdAsync(accountId, It.IsAny<CancellationToken>())).ReturnsAsync(account);
            _mockWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));

            var command = new ActivateAccountCommand { AccountId = accountId };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(account.Status == Domain.Enums.AccountStatus.Active);

            _mockRepository.Verify(x => x.GetByIdAsync(accountId, It.IsAny<CancellationToken>()), Times.Once);
            _mockWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Should_Throw_When_Account_Not_Found()
        {
            // Arrange
            var accountId = Guid.NewGuid();

            _mockRepository.Setup(x => x.GetByIdAsync(accountId, It.IsAny<CancellationToken>())).ReturnsAsync((Account?)null);
            _mockWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));

            var command = new ActivateAccountCommand { AccountId = accountId };

            // Act and Assert
            Assert.ThrowsAsync<AccountNotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _mockRepository.Verify(x => x.GetByIdAsync(accountId, It.IsAny<CancellationToken>()), Times.Once);
            _mockWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public void Should_Throw_When_Account_Is_Already_Active()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var account = Account.Create("123456789012", customerId, Domain.Enums.AccountType.Savings, "INR", 1000, "test-created-by");

            _mockRepository.Setup(x => x.GetByIdAsync(accountId, It.IsAny<CancellationToken>())).ReturnsAsync(account);
            _mockWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()));

            var command = new ActivateAccountCommand { AccountId = accountId };
            account.Activate();

            // Act and Assert
            Assert.ThrowsAsync<InvalidAccountStatusException>(() => _handler.Handle(command, CancellationToken.None));

            _mockRepository.Verify(x => x.GetByIdAsync(accountId, It.IsAny<CancellationToken>()), Times.Once);
            _mockWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
