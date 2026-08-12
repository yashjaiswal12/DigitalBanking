using DigitalBanking.Application.Features.Accounts.DTOs;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DigitalBanking.Application.Features.Accounts.Queries.GetAccountById
{
    public class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, Account>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<GetAccountByIdQueryHandler> _logger;

        public GetAccountByIdQueryHandler(IAccountRepository accountRepository, ILogger<GetAccountByIdQueryHandler> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }

        public async Task<Account> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken)
                ?? throw new AccountNotFoundException();

            _logger.Log(LogLevel.Information, "Account information retrieved successfully");

            return new Account
            {
                Id = account.Id,
                AccountNumber = account.AccountNumber,
                CustomerId = account.CustomerId,
                Type = account.Type,
                Currency = account.Currency,
                Status = account.Status,
                LedgerBalance = account.LedgerBalance,
                AvailableBalance = account.AvailableBalance,
                MinimumBalance = account.MinimumBalance,
                OpenedOn = account.OpenedOn
            };
        }
    }
}
