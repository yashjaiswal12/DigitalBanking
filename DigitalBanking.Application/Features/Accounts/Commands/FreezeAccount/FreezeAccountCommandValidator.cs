using FluentValidation;

namespace DigitalBanking.Application.Features.Accounts.Commands.FreezeAccount
{
    public class FreezeAccountCommandValidator : AbstractValidator<FreezeAccountCommand>
    {
        public FreezeAccountCommandValidator()
        {
            RuleFor(x => x.AccountId).NotEmpty();
        }
    }
}
