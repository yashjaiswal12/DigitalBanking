using DigitalBanking.Application.Features.Accounts.DTOs;

namespace DigitalBanking.Application.Interfaces.Persistence
{
    public interface IAccountQueries
    {
        Task<List<AccountDto>> SearchAccountsAsync(SearchAccountCriteria searchCriteria, CancellationToken cancellationToken);
    }
}
