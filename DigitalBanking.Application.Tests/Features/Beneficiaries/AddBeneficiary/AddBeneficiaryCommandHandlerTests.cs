using DigitalBanking.Application.Features.Beneficiaries.Commands.AddBeneficiary;
using DigitalBanking.Application.Interfaces.Common;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Domain.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace DigitalBanking.Application.Tests.Features.Beneficiaries.AddBeneficiary
{
    public class AddBeneficiaryCommandHandlerTests
    {
        private readonly AddBeneficiaryCommandHandler _handler;
        private readonly Mock<IBeneficiaryRepository> _mockBeneficiaryRepo;
        private readonly Mock<IUnitOfWork> _mockWork;
        private readonly Mock<ICurrentUserService> _mockUserService;
        private readonly Mock<IAccountRepository> _mockAccountRepo;

        public AddBeneficiaryCommandHandlerTests()
        {
            _mockBeneficiaryRepo = new Mock<IBeneficiaryRepository>();
            _mockWork = new Mock<IUnitOfWork>();
            _mockUserService = new Mock<ICurrentUserService>();
            _mockAccountRepo = new Mock<IAccountRepository>();
            
            _handler = new AddBeneficiaryCommandHandler(_mockBeneficiaryRepo.Object, _mockUserService.Object, _mockWork.Object,
                NullLogger<AddBeneficiaryCommandHandler>.Instance, _mockAccountRepo.Object);
        }

        [Fact]
        public async Task Should_Create_Beneficiary_When_Request_Is_Valid()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var accountNumber = "test-account-number";
            var account = Account.Create(accountNumber, customerId, Domain.Enums.AccountType.Savings, "INR", 1000, customerId.ToString());

            account.Activate();

            _mockUserService.Setup(x => x.UserId).Returns(customerId);
            _mockBeneficiaryRepo.Setup(x => x.BeneficiaryExistsAsync(customerId, accountNumber, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _mockAccountRepo.Setup(x => x.GetByAccountNumberAsync(accountNumber, It.IsAny<CancellationToken>())).ReturnsAsync(account);
            _mockWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var command = new AddBeneficiaryCommand 
            { 
                AccountNumber =  accountNumber,
                BeneficiaryBankName = "test-bank",
                BankCode = "123",
                BeneficiaryName = "test-name"
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeEmpty();

            _mockBeneficiaryRepo.Verify(x => x.BeneficiaryExistsAsync(customerId, accountNumber, It.IsAny<CancellationToken>()), Times.Once);
            _mockWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mockAccountRepo.Verify(x => x.GetByAccountNumberAsync(accountNumber, It.IsAny<CancellationToken>()), Times.Once);
            
            _mockBeneficiaryRepo.Verify(x => x.AddBeneficiaryAsync(It.Is<Beneficiary>(b => b.BeneficiaryAccountNumber == command.AccountNumber
                && b.BeneficiaryBankName == command.BeneficiaryBankName && b.BeneficiaryBankCode == command.BankCode
                && b.BeneficiaryName == command.BeneficiaryName && b.CustomerId == customerId && b.AccountId == account.Id)
                , It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_When_Duplicate_Beneficiary_Exists()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var accountNumber = "test-account-number";

            _mockUserService.Setup(x => x.UserId).Returns(customerId);
            _mockBeneficiaryRepo.Setup(x => x.BeneficiaryExistsAsync(customerId, accountNumber, It.IsAny<CancellationToken>())).ReturnsAsync(true);

            var command = new AddBeneficiaryCommand
            {
                AccountNumber = accountNumber,
                BeneficiaryBankName = "test-bank",
                BankCode = "123",
                BeneficiaryName = "test-name"
            };

            // Act and Assert
            await Assert.ThrowsAsync<DuplicateBeneficiaryException>(() => _handler.Handle(command, CancellationToken.None));

            _mockBeneficiaryRepo.Verify(x => x.BeneficiaryExistsAsync(customerId, accountNumber, It.IsAny<CancellationToken>()), Times.Once);
            _mockWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _mockAccountRepo.Verify(x => x.GetByAccountNumberAsync(accountNumber, It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Should_Throw_When_Account_Not_Found()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var accountNumber = "test-account-number";

            _mockUserService.Setup(x => x.UserId).Returns(customerId);
            _mockBeneficiaryRepo.Setup(x => x.BeneficiaryExistsAsync(customerId, accountNumber, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _mockAccountRepo.Setup(x => x.GetByAccountNumberAsync(accountNumber, It.IsAny<CancellationToken>())).ReturnsAsync((Account?)null);

            var command = new AddBeneficiaryCommand
            {
                AccountNumber = accountNumber,
                BeneficiaryBankName = "test-bank",
                BankCode = "123",
                BeneficiaryName = "test-name"
            };

            // Act and Assert
            await Assert.ThrowsAsync<AccountNotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _mockBeneficiaryRepo.Verify(x => x.BeneficiaryExistsAsync(customerId, accountNumber, It.IsAny<CancellationToken>()), Times.Once);
            _mockAccountRepo.Verify(x => x.GetByAccountNumberAsync(accountNumber, It.IsAny<CancellationToken>()), Times.Once);
            _mockWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Should_Throw_When_Account_Status_Is_Not_Correct()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var accountNumber = "test-account-number";
            var account = Account.Create(accountNumber, customerId, Domain.Enums.AccountType.Savings, "INR", 1000, customerId.ToString());

            _mockUserService.Setup(x => x.UserId).Returns(customerId);
            _mockBeneficiaryRepo.Setup(x => x.BeneficiaryExistsAsync(customerId, accountNumber, It.IsAny<CancellationToken>())).ReturnsAsync(false);
            _mockAccountRepo.Setup(x => x.GetByAccountNumberAsync(accountNumber, It.IsAny<CancellationToken>())).ReturnsAsync(account);

            var command = new AddBeneficiaryCommand
            {
                AccountNumber = accountNumber,
                BeneficiaryBankName = "test-bank",
                BankCode = "123",
                BeneficiaryName = "test-name"
            };

            // Act and Assert
            await Assert.ThrowsAsync<InvalidAccountStatusException>(() => _handler.Handle(command, CancellationToken.None));

            _mockBeneficiaryRepo.Verify(x => x.BeneficiaryExistsAsync(customerId, accountNumber, It.IsAny<CancellationToken>()), Times.Once);
            _mockAccountRepo.Verify(x => x.GetByAccountNumberAsync(accountNumber, It.IsAny<CancellationToken>()), Times.Once);
            _mockWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
