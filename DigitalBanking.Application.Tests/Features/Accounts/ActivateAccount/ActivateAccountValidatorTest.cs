using DigitalBanking.Application.Features.Accounts.Commands.ActivateAccount;
using FluentAssertions;

namespace DigitalBanking.Application.Tests.Features.Accounts.ActivateAccount
{
    public class ActivateAccountValidatorTest
    {
        private readonly ActivateAccountCommandValidator _validator;

        public ActivateAccountValidatorTest()
        {
            _validator = new ActivateAccountCommandValidator();
        }

        [Fact]
        public void Should_Be_True_If_Account_Id_Is_Valid()
        {
            var accountId = Guid.NewGuid();

            var command = new ActivateAccountCommand { AccountId = accountId };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Be_False_If_Account_Id_Is_InValid()
        {
            var accountId = Guid.Empty;

            var command = new ActivateAccountCommand { AccountId = accountId };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
        }
    }
}
