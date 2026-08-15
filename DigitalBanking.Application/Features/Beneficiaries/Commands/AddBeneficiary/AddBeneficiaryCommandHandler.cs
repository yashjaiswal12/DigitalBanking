using DigitalBanking.Application.Interfaces.Common;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace DigitalBanking.Application.Features.Beneficiaries.Commands.AddBeneficiary
{
    public class AddBeneficiaryCommandHandler : IRequestHandler<AddBeneficiaryCommand, Guid>
    {
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AddBeneficiaryCommandHandler> _logger;

        public AddBeneficiaryCommandHandler(IBeneficiaryRepository beneficiaryRepository, ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork, ILogger<AddBeneficiaryCommandHandler> logger, IAccountRepository accountRepository)
        {
            _beneficiaryRepository = beneficiaryRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _accountRepository = accountRepository;
        }

        public async Task<Guid> Handle(AddBeneficiaryCommand request, CancellationToken cancellationToken)
        {
            var customerId = _currentUserService.UserId;

            var beneficiaryExists = await _beneficiaryRepository.BeneficiaryExistsAsync(customerId, request.AccountNumber, cancellationToken);
            if (beneficiaryExists)
                throw new DuplicateBeneficiaryException();

            var account = await _accountRepository.GetByAccountNumberAsync(request.AccountNumber, cancellationToken)
                ?? throw new AccountNotFoundException();

            if (account.Status != Domain.Enums.AccountStatus.Active)
                throw new InvalidAccountStatusException("Account is not active");

            if (account.CustomerId == customerId)
                throw new OwnAccountBeneficiaryException();

            var beneficiary = Beneficiary.Create(customerId, account.Id, request.BeneficiaryName, request.BankCode, request.AccountNumber,
                request.BeneficiaryBankName);

            beneficiary.Verify();

            await _beneficiaryRepository.AddBeneficiaryAsync(beneficiary, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.Log(LogLevel.Information, "Beneficiary added successfully with ID {id}", beneficiary.Id);

            return beneficiary.Id;
        }
    }
}
