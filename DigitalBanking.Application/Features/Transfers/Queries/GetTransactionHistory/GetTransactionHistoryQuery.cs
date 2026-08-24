using DigitalBanking.Application.Common.Pagination;
using DigitalBanking.Application.Features.Transfers.DTOs;
using DigitalBanking.Domain.Enums;
using MediatR;

namespace DigitalBanking.Application.Features.Transfers.Queries.GetTransactionHistory
{
    public class GetTransactionHistoryQuery : PaginationRequest, IRequest<PagedResult<TransactionHistoryDto>>
    {
        public Guid AccountId { get; init; }
        public DateTime? FromDateUtc { get; init; }
        public DateTime? ToDateUtc { get; init; }
        public decimal? MinAmount { get; init; }
        public decimal? MaxAmount { get; init; }
        public string? Search { get; init; }
        public TransactionType? Type { get; init; } = TransactionType.InternalTransfer;
        public TransactionStatus? Status { get; init; } = TransactionStatus.Completed;
        public TransactionSortField? SortBy { get; init; } = TransactionSortField.CreatedAt;
        public bool Descending { get; init; } = true;
    }
}
