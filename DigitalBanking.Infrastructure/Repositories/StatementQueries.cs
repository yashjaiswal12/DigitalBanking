using DigitalBanking.Application.Features.Statements.DTOs;
using DigitalBanking.Application.Interfaces.Common;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Exceptions;
using DigitalBanking.Infrastructure.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace DigitalBanking.Infrastructure.Repositories
{
    public class StatementQueries : IStatementQueries
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICustomerRepository _customerRepository;

        public StatementQueries(ICustomerRepository customerRepository, IAccountRepository accountRepository, ICurrentUserService currentUserService,
            ITransactionRepository transactionRepository)
        {
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _currentUserService = currentUserService;
            _transactionRepository = transactionRepository;
        }

        public async Task<AccountStatementDto> GenerateAsync(Guid accountId, DateTime fromDateUtc, DateTime toDateUtc, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByIdAsync(accountId, cancellationToken)
                ?? throw new AccountNotFoundException();

            if (account.CustomerId != _currentUserService.UserId)
                throw new ForbiddenException();

            var transactionQuery = _transactionRepository.GetQueryable();
            transactionQuery = transactionQuery.Where(x => x.SourceAccountId == accountId || x.DestinationAccountId == accountId);
            transactionQuery = transactionQuery.Where(x => x.CreatedAtUtc >= fromDateUtc && x.CreatedAtUtc <= toDateUtc);

            var transactions = await transactionQuery.OrderBy(x => x.CreatedBy)
                .Select(x => new StatementTransactionDto
                {
                    Amount = x.Amount,
                    CompletedAtUtc = x.CompletedAtUtc,
                    CreatedAtUtc = x.CreatedAtUtc,
                    DestinationAccountId = x.DestinationAccountId,
                    SourceAccountId = x.SourceAccountId,
                    ReferenceNumber = x.ReferenceNumber,
                    TransactionId = x.Id,
                    Status = x.Status.ToString(),
                    Type = x.Type.ToString()
                }).ToListAsync(cancellationToken);

            var totalCredits = await transactionQuery.Where(x => x.DestinationAccountId == accountId).SumAsync(x => (decimal)x.Amount, cancellationToken);
            var totalDebits = await transactionQuery.Where(x => x.SourceAccountId == accountId).SumAsync(x => (decimal)x.Amount, cancellationToken);

            var openingBalance = account.AvailableBalance - totalCredits + totalDebits;
            var closingBalance = account.AvailableBalance + totalCredits - totalDebits;

            var summary = new StatementSummaryDto
            {
                TotalCredits = totalCredits,
                TotalDebits = totalDebits,
                TotalTransactions = transactionQuery.Count()
            };

            return new AccountStatementDto
            {
                AccountId = accountId,
                AccountNumber = account.AccountNumber,
                ClosingBalance = closingBalance,
                OpeningBalance = openingBalance,
                FromDateUtc = fromDateUtc,
                ToDateTimeUtc = toDateUtc,
                StatementSummary = summary,
                Transactions = transactions,
                CustomerName = account.CustomerId.ToString()
            };
        }
    }
}
