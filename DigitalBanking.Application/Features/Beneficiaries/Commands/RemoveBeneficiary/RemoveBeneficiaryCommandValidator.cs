using FluentValidation;

namespace DigitalBanking.Application.Features.Beneficiaries.Commands.RemoveBeneficiary
{
    public class RemoveBeneficiaryCommandValidator : AbstractValidator<RemoveBeneficiaryCommand>
    {
        public RemoveBeneficiaryCommandValidator()
        {
            RuleFor(x => x.BeneficiaryId).NotEmpty().WithMessage("Beneficiary id is required");
        }
    }
}
