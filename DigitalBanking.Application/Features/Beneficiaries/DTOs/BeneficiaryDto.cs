using DigitalBanking.Domain.Enums;

namespace DigitalBanking.Application.Features.Beneficiaries.DTOs
{
    public class BeneficiaryDto
    {
        public Guid Id { get; set; }
        public string BeneficiaryName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string BankCode { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public BeneficiaryStatus Status { get; set; }
        public bool IsVerified { get; set; }
        public DateTime? VerifiedAt { get; set; }
    }
}
