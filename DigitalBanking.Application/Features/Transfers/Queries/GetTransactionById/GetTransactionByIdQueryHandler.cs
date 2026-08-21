using DigitalBanking.Application.Features.Transfers.DTOs;
using DigitalBanking.Application.Interfaces.Common;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Exceptions;
using MediatR;

namespace DigitalBanking.Application.Features.Transfers.Queries.GetTransactionById
{
    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, TransactionDetailDto>
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAccountRepository _accountRepository;

        public GetTransactionByIdQueryHandler(ITransactionRepository transactionRepository, ICurrentUserService currentUserService, IAccountRepository accountRepository)
        {
            _transactionRepository = transactionRepository;
            _currentUserService = currentUserService;
            _accountRepository = accountRepository;
        }

        public async Task<TransactionDetailDto> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var transaction = await _transactionRepository.GetByTransactionIdAsync(request.TransactionId, cancellationToken)
                ?? throw new InvalidOperationException("Invalid transaction id");

            var account = await _accountRepository.GetByIdAsync(transaction.SourceAccountId, cancellationToken)
                ?? throw new Exception();
            if (account.CustomerId != _currentUserService.UserId)
                throw new ForbiddenException();

            return new TransactionDetailDto
            {
                Amount = transaction.Amount,
                CompletedAtUtc = transaction.CompletedAtUtc,
                CreatedAtUtc = transaction.CreatedAtUtc,
                DestinationAccountId = transaction.DestinationAccountId,
                SourceAccountId = transaction.SourceAccountId,
                FailureReason = transaction.FailureReason,
                ReferenceNumber = transaction.ReferenceNumber,
                Status = transaction.Status.ToString(),
                TransactionId = transaction.Id,
                Type = transaction.Type.ToString()
            };
        }
    }
}
