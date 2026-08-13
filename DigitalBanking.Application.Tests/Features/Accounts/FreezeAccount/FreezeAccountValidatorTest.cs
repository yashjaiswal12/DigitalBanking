using DigitalBanking.Application.Features.Accounts.Commands.ActivateAccount;
using DigitalBanking.Application.Features.Accounts.Commands.FreezeAccount;
using FluentAssertions;

namespace DigitalBanking.Application.Tests.Features.Accounts.FreezeAccount
{
    public class FreezeAccountValidatorTest
    {
        private readonly FreezeAccountCommandValidator _validator;

        public FreezeAccountValidatorTest()
        {
            _validator = new FreezeAccountCommandValidator();
        }

        [Fact]
        public void Should_Be_True_If_Account_Id_Is_Valid()
        {
            var accountId = Guid.NewGuid();

            var command = new FreezeAccountCommand { AccountId = accountId };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Be_False_If_Account_Id_Is_InValid()
        {
            var accountId = Guid.Empty;

            var command = new FreezeAccountCommand { AccountId = accountId };

            var result = _validator.Validate(command);

            result.IsValid.Should().BeFalse();
        }
    }
}
