using FluentValidation;

namespace DigitalBanking.Application.Features.Accounts.Commands.CloseAccount
{
    public class CloseAccountCommandValidator : AbstractValidator<CloseAccountCommand>
    {
        public CloseAccountCommandValidator()
        {
            RuleFor(x => x.AccountId).NotEmpty();
        }
    }
}
