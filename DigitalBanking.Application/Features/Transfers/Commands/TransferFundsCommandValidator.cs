using FluentValidation;

namespace DigitalBanking.Application.Features.Transfers.Commands
{
    public class TransferFundsCommandValidator : AbstractValidator<TransferFundsCommand>
    {
        public TransferFundsCommandValidator()
        {
            RuleFor(x => x.SourceAccountId).NotEmpty().WithMessage("Source account id is required");
            RuleFor(x => x.DestinationAccountId).NotEmpty().WithMessage("Destination account id is required")
                .NotEqual(x => x.SourceAccountId).WithMessage("Self transfer is not allowed");
            RuleFor(x => x.Amount).NotEmpty().GreaterThanOrEqualTo(1);
        }
    }
}
