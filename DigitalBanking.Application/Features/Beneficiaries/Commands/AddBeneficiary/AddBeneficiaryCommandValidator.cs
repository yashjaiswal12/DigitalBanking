using FluentValidation;

namespace DigitalBanking.Application.Features.Beneficiaries.Commands.AddBeneficiary
{
    public class AddBeneficiaryCommandValidator : AbstractValidator<AddBeneficiaryCommand>
    {
        public AddBeneficiaryCommandValidator()
        {
            RuleFor(x => x.AccountNumber).NotEmpty().WithMessage("Account number is required").Length(8, 20);
            RuleFor(x => x.BeneficiaryName).NotEmpty().WithMessage("Beneficiary name should not be empty").Length(2, 100);
            RuleFor(x => x.BankCode).NotEmpty().Length(3, 20);
            RuleFor(x => x.BeneficiaryBankName).NotEmpty().WithMessage("Beneficiary bank name is required");
        }
    }
}
