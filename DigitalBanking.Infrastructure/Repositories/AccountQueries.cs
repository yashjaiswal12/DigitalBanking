using DigitalBanking.Application.Common.Pagination;
using DigitalBanking.Application.Features.Accounts.DTOs;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DigitalBanking.Infrastructure.Repositories
{
    public class AccountQueries : IAccountQueries
    {
        private readonly ApplicationDbContext _context;

        public AccountQueries(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<AccountDto>> SearchAccountsAsync(SearchAccountCriteria searchCriteria, CancellationToken cancellationToken)
        {
            var query = _context.Accounts.AsNoTracking();

            if (searchCriteria.CustomerId.HasValue)
                query = query.Where(x => x.CustomerId == searchCriteria.CustomerId.Value);

            if (!string.IsNullOrWhiteSpace(searchCriteria.AccountNumber))
                query = query.Where(x => x.AccountNumber == searchCriteria.AccountNumber);

            if (!string.IsNullOrWhiteSpace(searchCriteria.Currency))
            {
                var normalizedCurrency = searchCriteria.Currency.ToUpperInvariant();
                query = query.Where(x => x.Currency == normalizedCurrency);
            }

            if (searchCriteria.Status.HasValue)
                query = query.Where(x => x.Status == searchCriteria.Status.Value);

            if (searchCriteria.Type.HasValue)
                query = query.Where(x => x.Type == searchCriteria.Type.Value);

            var totalCount = await query.CountAsync(cancellationToken);
            var skip = (searchCriteria.PageNumber - 1) * searchCriteria.PageSize;

            var items = await query.OrderBy(x => x.AccountNumber)
                .ThenBy(x => x.Id)
                .Skip(skip)
                .Take(searchCriteria.PageSize)
                .Select(x => new AccountDto
                {
                    Id = x.Id,
                    AccountNumber = x.AccountNumber,
                    CustomerId = x.CustomerId,
                    Currency = x.Currency,
                    Type = x.Type,
                    Status = x.Status,
                    AvailableBalance = x.AvailableBalance,
                    LedgerBalance = x.LedgerBalance,
                    MinimumBalance = x.MinimumBalance,
                    OpenedOn = x.OpenedOn,
                    RowVersion = Convert.ToBase64String(x.RowVersion)
                }).ToListAsync(cancellationToken);

            return PagedResult<AccountDto>.Create(items, searchCriteria.PageNumber, searchCriteria.PageSize, totalCount);
        }
    }
}
