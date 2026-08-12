using FluentValidation;

namespace DigitalBanking.Application.Features.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.Currency).NotEmpty().MaximumLength(3);
            RuleFor(x => x.InitialBalance).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Type).IsInEnum();
        }
    }
}
