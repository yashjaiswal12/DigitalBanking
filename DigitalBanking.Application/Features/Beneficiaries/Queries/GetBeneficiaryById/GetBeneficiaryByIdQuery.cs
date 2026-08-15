using DigitalBanking.Application.Features.Beneficiaries.DTOs;
using MediatR;

namespace DigitalBanking.Application.Features.Beneficiaries.Queries.GetBeneficiaryById
{
    public class GetBeneficiaryByIdQuery : IRequest<BeneficiaryDto>
    {
        public Guid BeneficiaryId { get; set; }
    }
}
