using DigitalBanking.Application.Features.Beneficiaries.Queries.GetBeneficiaries;
using DigitalBanking.Application.Interfaces.Common;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Domain.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace DigitalBanking.Application.Tests.Features.Beneficiaries.GetBeneficiaries
{
    public class GetBeneficiariesQueryHandlerTests
    {
        private readonly Mock<ICurrentUserService> _mockService;
        private readonly Mock<IBeneficiaryRepository> _mockRepo;
        private readonly GetBeneficiariesQueryHandler _handler;

        public GetBeneficiariesQueryHandlerTests()
        {
            _mockRepo = new Mock<IBeneficiaryRepository>();
            _mockService = new Mock<ICurrentUserService>();
            _handler = new GetBeneficiariesQueryHandler(_mockService.Object, _mockRepo.Object, NullLogger<GetBeneficiariesQueryHandler>.Instance);
        }

        [Fact]
        public async Task Should_Get_Beneficiaries()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            List<Beneficiary> beneficiaries = new List<Beneficiary>
            {
                Beneficiary.Create(customerId, Guid.NewGuid(), "testName", "bankCode", "accNumber", "bankName"),
                Beneficiary.Create(customerId, Guid.NewGuid(), "testName1", "bankCode1", "accNumber1", "bankName1"),
                Beneficiary.Create(customerId, Guid.NewGuid(), "testName2", "bankCode2", "accNumber2", "bankName2"),
                Beneficiary.Create(customerId, Guid.NewGuid(), "testName3", "bankCode3", "accNumber3", "bankName3")
            };

            _mockRepo.Setup(x => x.GetBeneficiariesByCustomerIdAsync(customerId, It.IsAny<CancellationToken>())).ReturnsAsync(beneficiaries);
            _mockService.Setup(x => x.UserId).Returns(customerId);

            var query = new GetBeneficiariesQuery { };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();

            Assert.Equal(result[0].BeneficiaryName, beneficiaries[0].BeneficiaryName);
            Assert.Equal(result[0].BankName, beneficiaries[0].BeneficiaryBankName);
            Assert.Equal(result[1].BeneficiaryName, beneficiaries[1].BeneficiaryName);
            Assert.Equal(result[2].AccountNumber, beneficiaries[2].BeneficiaryAccountNumber);

            _mockRepo.Verify(x => x.GetBeneficiariesByCustomerIdAsync(customerId, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Should_Throw_When_Beneficiaries_Not_Found()
        {
            // Arrange
            var customerId = Guid.NewGuid();

            List<Beneficiary> beneficiaries = new List<Beneficiary>();

            _mockRepo.Setup(x => x.GetBeneficiariesByCustomerIdAsync(customerId, It.IsAny<CancellationToken>())).ReturnsAsync(beneficiaries);
            _mockService.Setup(x => x.UserId).Returns(customerId);

            var query = new GetBeneficiariesQuery { };

            // Act and Assert
            await Assert.ThrowsAsync<BeneficiaryNotFoundException>(() => _handler.Handle(query, CancellationToken.None)); 

            _mockRepo.Verify(x => x.GetBeneficiariesByCustomerIdAsync(customerId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
