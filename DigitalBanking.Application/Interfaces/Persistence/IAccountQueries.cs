using DigitalBanking.Application.Common.Pagination;
using DigitalBanking.Application.Features.Accounts.DTOs;

namespace DigitalBanking.Application.Interfaces.Persistence
{
    public interface IAccountQueries
    {
        Task<PagedResult<AccountDto>> SearchAccountsAsync(SearchAccountCriteria searchCriteria, CancellationToken cancellationToken);
    }
}
