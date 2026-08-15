using DigitalBanking.Application.Features.Beneficiaries.Queries.GetBeneficiaries;
using DigitalBanking.Application.Features.Beneficiaries.Queries.GetBeneficiaryById;
using DigitalBanking.Application.Interfaces.Common;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace DigitalBanking.Application.Tests.Features.Beneficiaries.GetBeneficiaryById
{
    public class GetBeneficiaryByIdQueryHandlerTests
    {
        private readonly Mock<ICurrentUserService> _mockService;
        private readonly Mock<IBeneficiaryRepository> _mockRepo;
        private readonly GetBeneficiaryByIdQueryHandler _handler;

        public GetBeneficiaryByIdQueryHandlerTests()
        {
            _mockRepo = new Mock<IBeneficiaryRepository>();
            _mockService = new Mock<ICurrentUserService>();
            _handler = new GetBeneficiaryByIdQueryHandler(_mockRepo.Object, NullLogger<GetBeneficiaryByIdQueryHandler>.Instance, _mockService.Object);
        }

        [Fact]
        public async Task Should_Get_Beneficiary()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var beneficiary = Beneficiary.Create(customerId, Guid.NewGuid(), "testName", "bankCode", "accNumber", "bankName");

            _mockRepo.Setup(x => x.GetBeneficiaryByIdAsync(beneficiary.Id, customerId, It.IsAny<CancellationToken>())).ReturnsAsync(beneficiary);
            _mockService.Setup(x => x.UserId).Returns(customerId);

            var query = new GetBeneficiaryByIdQuery { BeneficiaryId = beneficiary.Id };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(result.BeneficiaryName, beneficiary.BeneficiaryName);
            Assert.Equal(result.BankName, beneficiary.BeneficiaryBankName);
            Assert.Equal(result.BankCode, beneficiary.BeneficiaryBankCode);
            Assert.Equal(result.AccountNumber, beneficiary.BeneficiaryAccountNumber);

            _mockRepo.Verify(x => x.GetBeneficiaryByIdAsync(beneficiary.Id, customerId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_When_Beneficiary_Not_Found()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var beneficiaryId = Guid.NewGuid();

            _mockRepo.Setup(x => x.GetBeneficiaryByIdAsync(beneficiaryId, customerId, It.IsAny<CancellationToken>())).ReturnsAsync((Beneficiary?)null);
            _mockService.Setup(x => x.UserId).Returns(customerId);

            var query = new GetBeneficiaryByIdQuery { BeneficiaryId = beneficiaryId };

            // Act and Assert
            await Assert.ThrowsAsync<BeneficiaryNotFoundException>(() => _handler.Handle(query, CancellationToken.None));

            _mockRepo.Verify(x => x.GetBeneficiaryByIdAsync(beneficiaryId, customerId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
