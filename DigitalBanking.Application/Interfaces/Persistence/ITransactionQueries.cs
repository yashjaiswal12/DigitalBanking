using DigitalBanking.Application.Common.Pagination;
using DigitalBanking.Application.Features.Transfers.DTOs;
using DigitalBanking.Application.Features.Transfers.Queries.GetTransactionHistory;

namespace DigitalBanking.Application.Interfaces.Persistence
{
    public interface ITransactionQueries
    {
        Task<PagedResult<TransactionHistoryDto>> GetResultAsync(GetTransactionHistoryQuery query, CancellationToken cancellationToken);
    }
}
