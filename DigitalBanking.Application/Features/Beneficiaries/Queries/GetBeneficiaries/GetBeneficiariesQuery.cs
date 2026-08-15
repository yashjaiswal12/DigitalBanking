using DigitalBanking.Application.Features.Beneficiaries.DTOs;
using MediatR;

namespace DigitalBanking.Application.Features.Beneficiaries.Queries.GetBeneficiaries
{
    public class GetBeneficiariesQuery : IRequest<List<BeneficiaryDto>>
    {
    }
}
