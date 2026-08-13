using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DigitalBanking.Application.Features.Accounts.Commands.ActivateAccount
{
    public class ActivateAccountCommandHandler : IRequestHandler<ActivateAccountCommand>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ActivateAccountCommandHandler> _logger;

        public ActivateAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork, ILogger<ActivateAccountCommandHandler> logger)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(ActivateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken)
                ?? throw new AccountNotFoundException();

            account.Activate();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.Log(LogLevel.Information, "Account activated successfully");
        }
    }
}
