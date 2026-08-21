using DigitalBanking.Application.Common.Pagination;
using DigitalBanking.Application.Features.Transfers.DTOs;
using DigitalBanking.Application.Features.Transfers.Queries.GetTransactionHistory;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DigitalBanking.Infrastructure.Repositories
{
    public class TransactionQueries : ITransactionQueries
    {
        private readonly ApplicationDbContext _context;

        public TransactionQueries(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<TransactionHistoryDto>> GetResultAsync(GetTransactionHistoryQuery query, CancellationToken cancellationToken)
        {
            var transactionQuery = _context.Transactions.AsNoTracking();

            transactionQuery = transactionQuery.Where(x => x.SourceAccountId == query.AccountId || x.DestinationAccountId == query.AccountId);

            if (query.FromDateUtc.HasValue)
                transactionQuery = transactionQuery.Where(x => x.CreatedAtUtc >= query.FromDateUtc);

            if (query.ToDateUtc.HasValue)
                transactionQuery = transactionQuery.Where(x => x.CreatedAtUtc <= query.ToDateUtc);

            if (query.Status.HasValue)
                transactionQuery = transactionQuery.Where(x => x.Status == query.Status);

            if (query.Type.HasValue)
                transactionQuery = transactionQuery.Where(x => x.Type == query.Type);

            if (query.MaxAmount.HasValue)
                transactionQuery = transactionQuery.Where(x => x.Amount <= query.MaxAmount);

            if (query.MinAmount.HasValue)
                transactionQuery = transactionQuery.Where(x => x.Amount >= query.MinAmount);

            if (!string.IsNullOrWhiteSpace(query.Search))
                transactionQuery = transactionQuery.Where(x => x.ReferenceNumber.Contains(query.Search));

            transactionQuery = ApplySorting(transactionQuery, query);

            var totalCount = await transactionQuery.CountAsync(cancellationToken);

            var items = await transactionQuery.Skip((query.Page-1) * query.PageSize).Take(query.PageSize)
                .Select(x => new TransactionHistoryDto
                {
                    Amount = x.Amount,
                    TransactionId = x.Id,
                    ReferenceNumber = x.ReferenceNumber,
                    Type = x.Type.ToString(),
                    Status = x.Status.ToString(),
                    CreatedAtUtc = x.CreatedAtUtc,
                    CompletedAtUtc = x.CompletedAtUtc,
                    SourceAccountId = x.SourceAccountId,
                    DestinationAccountId = x.DestinationAccountId
                })
                .ToListAsync(cancellationToken);

            return PagedResult<TransactionHistoryDto>.Create(items, query.Page, query.PageSize, totalCount);
        }

        private static IQueryable<Transaction> ApplySorting(IQueryable<Transaction> transaction, GetTransactionHistoryQuery query)
        {
            return query.SortBy switch
            {
                Domain.Enums.TransactionSortField.Amount => query.Descending ? transaction.OrderByDescending(x => x.Amount)
                    : transaction.OrderBy(x => x.Amount),


                Domain.Enums.TransactionSortField.Status => query.Descending ? transaction.OrderByDescending(x => x.Status)
                    : transaction.OrderBy(x => x.Status),

                Domain.Enums.TransactionSortField.ReferenceNumber => query.Descending ? transaction.OrderByDescending(x => x.ReferenceNumber)
                    : transaction.OrderBy(x => x.ReferenceNumber),

                _ => query.Descending ? transaction.OrderByDescending(x => x.CreatedAtUtc).ThenByDescending(x => x.Id)
                    : transaction.OrderBy(x => x.CreatedAtUtc).ThenBy(x => x.Id)
            };
        }
    }
}
