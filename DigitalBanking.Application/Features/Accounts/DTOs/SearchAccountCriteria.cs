using DigitalBanking.Domain.Enums;

namespace DigitalBanking.Application.Features.Accounts.DTOs
{
    public class SearchAccountCriteria
    {
        public string? AccountNumber { get; set; }
        public Guid? CustomerId { get; set; }
        public string? Currency { get; set; }
        public AccountType? Type { get; set; }
        public AccountStatus? Status { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
