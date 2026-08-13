using FluentValidation;

namespace DigitalBanking.Application.Features.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        public CreateAccountCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.Currency).NotEmpty().MaximumLength(3).Must(x => x == x.ToUpperInvariant())
                .WithMessage("Currency must be an uppercase ISO currency code.");
            RuleFor(x => x.InitialBalance).GreaterThanOrEqualTo(1000);
            RuleFor(x => x.Type).IsInEnum();
        }
    }
}
