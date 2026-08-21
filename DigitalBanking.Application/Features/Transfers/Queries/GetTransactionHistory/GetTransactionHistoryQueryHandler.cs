using DigitalBanking.Application.Common.Pagination;
using DigitalBanking.Application.Features.Transfers.DTOs;
using DigitalBanking.Application.Interfaces.Common;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Exceptions;
using MediatR;

namespace DigitalBanking.Application.Features.Transfers.Queries.GetTransactionHistory
{
    public class GetTransactionHistoryQueryHandler : IRequestHandler<GetTransactionHistoryQuery, PagedResult<TransactionHistoryDto>>
    {
        private readonly ITransactionQueries _transactionQueries;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAccountRepository _accountRepository;

        public GetTransactionHistoryQueryHandler(ITransactionQueries transactionQueries, IAccountRepository accountRepository, 
            ICurrentUserService currentUserService)
        {
            _transactionQueries = transactionQueries;
            _accountRepository = accountRepository;
            _currentUserService = currentUserService;
        }

        public async Task<PagedResult<TransactionHistoryDto>> Handle(GetTransactionHistoryQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken) ??
                throw new AccountNotFoundException();

            if (account.CustomerId != _currentUserService.UserId)
                throw new ForbiddenException();

            return await _transactionQueries.GetResultAsync(request, cancellationToken);
        }
    }
}
