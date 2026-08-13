using FluentValidation;

namespace DigitalBanking.Application.Features.Accounts.Commands.ActivateAccount
{
    public class ActivateAccountCommandValidator : AbstractValidator<ActivateAccountCommand>
    {
        public ActivateAccountCommandValidator()
        {
            RuleFor(x => x.AccountId).NotEmpty();
        }
    }
}
