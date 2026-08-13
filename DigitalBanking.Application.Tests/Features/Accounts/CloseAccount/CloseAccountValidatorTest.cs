using DigitalBanking.Application.Features.Accounts.Commands.ActivateAccount;
using DigitalBanking.Application.Features.Accounts.Commands.CloseAccount;
using FluentAssertions;

namespace DigitalBanking.Application.Tests.Features.Accounts.CloseAccount
{
    public class CloseAccountValidatorTest
    {
        private readonly CloseAccountCommandValidator _validator;

        public CloseAccountValidatorTest()
        {
            _validator = new CloseAccountCommandValidator();
        }

        [Fact]
        public void Should_Be_True_If_Account_Id_Is_Valid()
        {
            var accountId = Guid.NewGuid();

            var command = new CloseAccountCommand { AccountId = accountId };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Be_False_If_Account_Id_Is_InValid()
        {
            var accountId = Guid.Empty;

            var command = new CloseAccountCommand { AccountId = accountId };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
        }
    }
}
