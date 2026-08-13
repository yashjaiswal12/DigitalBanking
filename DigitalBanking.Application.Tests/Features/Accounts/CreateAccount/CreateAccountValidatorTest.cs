using DigitalBanking.Application.Features.Accounts.Commands.CreateAccount;
using DigitalBanking.Domain.Enums;
using FluentAssertions;

namespace DigitalBanking.Application.Tests.Features.Accounts.CreateAccount
{
    public class CreateAccountValidatorTest
    {
        private readonly CreateAccountCommandValidator _validator;

        public CreateAccountValidatorTest()
        {
            _validator = new CreateAccountCommandValidator();
        }

        [Fact]
        public void Should_Be_True_If_Correct_Validation()
        {
            // Arrange
            var command = new CreateAccountCommand
            {
                CustomerId = Guid.NewGuid(),
                Currency = "INR",
                InitialBalance = 1000,
                Type = Domain.Enums.AccountType.Savings
            };

            // Act
            var result = _validator.Validate(command);

            //Assert
            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [InlineData("123e4567-e89b-12d3-a456-426614174000", "inr", "10000", "Savings")]
        [InlineData("123e4567-e89b-12d3-a456-426614174000", "INR", "100", "Current")]
        public void Should_Be_False_If_InCorrect_Validation(string customerId, string currency, string initialBalance, string type)
        {
            // Arrange
            var command = new CreateAccountCommand
            {
                CustomerId = Guid.Parse(customerId),
                Currency = currency,
                InitialBalance = decimal.Parse(initialBalance),
                Type = Enum.Parse<AccountType>(type)
            };

            // Act
            var result = _validator.Validate(command);

            //Assert
            result.IsValid.Should().BeFalse();
        }
    }
}
