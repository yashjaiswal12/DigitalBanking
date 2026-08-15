using FluentValidation;

namespace DigitalBanking.Application.Features.Beneficiaries.Queries.GetBeneficiaryById
{
    public class GetBeneficiaryByIdQueryValidator : AbstractValidator<GetBeneficiaryByIdQuery>
    {
        public GetBeneficiaryByIdQueryValidator()
        {
            RuleFor(x => x.BeneficiaryId).NotEmpty().WithMessage("Beneficiary ID is required");
        }
    }
}
