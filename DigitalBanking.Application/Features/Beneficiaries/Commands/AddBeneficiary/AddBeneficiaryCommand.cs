using MediatR;

namespace DigitalBanking.Application.Features.Beneficiaries.Commands.AddBeneficiary
{
    public class AddBeneficiaryCommand : IRequest<Guid>
    {
        public string BeneficiaryName { get; set; } = string.Empty;
        public string BeneficiaryBankName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string BankCode { get; set; } = string.Empty;
    }
}
