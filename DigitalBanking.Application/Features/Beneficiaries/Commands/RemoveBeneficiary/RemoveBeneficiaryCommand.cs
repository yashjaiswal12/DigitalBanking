using MediatR;

namespace DigitalBanking.Application.Features.Beneficiaries.Commands.RemoveBeneficiary
{
    public class RemoveBeneficiaryCommand : IRequest
    {
        public Guid BeneficiaryId { get; set; }
    }
}
