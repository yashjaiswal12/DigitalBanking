using DigitalBanking.Application.Features.Transfers.DTOs;
using DigitalBanking.Application.Interfaces.Common;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Domain.Exceptions;
using DigitalBanking.Domain.Enums;
using MediatR;
using DigitalBanking.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace DigitalBanking.Application.Features.Transfers.Commands
{
    public class TransferFundsCommandHandler : IRequestHandler<TransferFundsCommand, TransferFundsDto>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IReferenceNumberGenerator _referenceNumberGenerator;
        private Transaction? _transaction;

        public TransferFundsCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork, ITransactionRepository transactionRepository,
            ICurrentUserService currentUserService, IReferenceNumberGenerator referenceNumberGenerator)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _transactionRepository = transactionRepository;
            _currentUserService = currentUserService;
            _referenceNumberGenerator = referenceNumberGenerator;
        }

        public async Task<TransferFundsDto> Handle(TransferFundsCommand request, CancellationToken cancellationToken)
        {
            var sourceAccount = await _accountRepository.GetByIdAsync(request.SourceAccountId, cancellationToken) ??
                throw new AccountNotFoundException();

            var destinationAccount = await _accountRepository.GetByIdAsync(request.DestinationAccountId, cancellationToken) ??
                throw new AccountNotFoundException();

            var customerId = _currentUserService.UserId;
            if (customerId != sourceAccount.CustomerId)
                throw new ForbiddenException();

            var referenceNumber = await _referenceNumberGenerator.GenerateAsync(cancellationToken);

            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                sourceAccount.Debit(request.Amount);
                destinationAccount.Credit(request.Amount);

                _transaction = Transaction.Create(referenceNumber, sourceAccount.Id, destinationAccount.Id, request.Amount, TransactionType.InternalTransfer);
                await _transactionRepository.AddAsync(_transaction, cancellationToken);
                _transaction.MarkAsCompleted();

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                _transaction?.MarkAsFailed("Failed to transfer funds");

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                throw new DbUpdateConcurrencyException("Account balance changed during transfer.");
            }

            return new TransferFundsDto
            {
                TransactionId = _transaction.Id,
                Status = _transaction.Status.ToString(),
                ReferenceNumber = _transaction.ReferenceNumber
            };
        }
    }
}
