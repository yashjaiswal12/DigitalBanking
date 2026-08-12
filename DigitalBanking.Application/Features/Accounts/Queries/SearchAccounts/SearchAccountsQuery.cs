using DigitalBanking.Application.Features.Accounts.DTOs;
using DigitalBanking.Domain.Enums;
using MediatR;

namespace DigitalBanking.Application.Features.Accounts.Queries.SearchAccounts
{
    public class SearchAccountsQuery : IRequest<List<Account>>
    {
        public string? AccountNumber { get; set; }
        public Guid? CustomerId { get; set; }
        public AccountType? Type { get; set; }
        public string? Currency { get; set; }
        public AccountStatus? Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
