using DigitalBanking.Domain.Enums;

namespace DigitalBanking.Application.Features.Accounts.DTOs
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public Guid CustomerId { get; set; }
        public AccountType Type { get; set; }
        public AccountStatus Status { get; set; }
        public string Currency { get; set; } = string.Empty;
        public decimal LedgerBalance { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal MinimumBalance { get; set; }
        public DateTimeOffset? OpenedOn { get; set; }
        public string RowVersion { get; set; } = string.Empty;
    }
}
