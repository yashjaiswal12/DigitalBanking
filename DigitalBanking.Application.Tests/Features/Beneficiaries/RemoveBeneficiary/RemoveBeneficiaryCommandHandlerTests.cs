using DigitalBanking.Application.Features.Beneficiaries.Commands.RemoveBeneficiary;
using DigitalBanking.Application.Interfaces.Common;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace DigitalBanking.Application.Tests.Features.Beneficiaries.RemoveBeneficiary
{
    public class RemoveBeneficiaryCommandHandlerTests
    {
        private readonly RemoveBeneficiaryCommandHandler _handler;
        private readonly Mock<IBeneficiaryRepository> _mockBeneficiaryRepo;
        private readonly Mock<IUnitOfWork> _mockWork;
        private readonly Mock<ICurrentUserService> _mockService;

        public RemoveBeneficiaryCommandHandlerTests()
        {
            _mockBeneficiaryRepo = new Mock<IBeneficiaryRepository>();
            _mockWork = new Mock<IUnitOfWork>();
            _mockService = new Mock<ICurrentUserService>();
            _handler = new RemoveBeneficiaryCommandHandler(_mockWork.Object, _mockBeneficiaryRepo.Object, _mockService.Object,
                NullLogger<RemoveBeneficiaryCommandHandler>.Instance);
        }

        [Fact]
        public async Task Should_Remove_Beneficiary()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var accountId = Guid.NewGuid();
            var beneficiary = Beneficiary.Create(customerId, accountId, "test-name", "ABC", "test-account-num", "test-bank-name");

            _mockBeneficiaryRepo.Setup(x => x.GetBeneficiaryByIdAsync(beneficiary.Id, customerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(beneficiary);
            _mockService.Setup(x => x.UserId).Returns(customerId);
            _mockWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var command = new RemoveBeneficiaryCommand { BeneficiaryId = beneficiary.Id };

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockBeneficiaryRepo.Verify(x => x.GetBeneficiaryByIdAsync(beneficiary.Id, customerId, It.IsAny<CancellationToken>()), Times.Once);
            _mockWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mockBeneficiaryRepo.Verify(x => x.RemoveBeneficiary(beneficiary), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_When_Beneficiary_Not_Found()
        {
            // Arrange

            _mockBeneficiaryRepo.Setup(x => x.GetBeneficiaryByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Beneficiary?)null);
            _mockService.Setup(x => x.UserId).Returns(It.IsAny<Guid>());
            _mockWork.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var command = new RemoveBeneficiaryCommand { BeneficiaryId = It.IsAny<Guid>() };

            // Act and Assert
            await Assert.ThrowsAsync<BeneficiaryNotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _mockBeneficiaryRepo.Verify(x => x.GetBeneficiaryByIdAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockWork.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
            _mockBeneficiaryRepo.Verify(x => x.RemoveBeneficiary(It.IsAny<Beneficiary>()), Times.Never);
        }
    }
}
