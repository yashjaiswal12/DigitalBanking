using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DigitalBanking.Application.Features.Accounts.Commands.CloseAccount
{
    public class CloseAccountCommandHandler : IRequestHandler<CloseAccountCommand>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CloseAccountCommandHandler> _logger;

        public CloseAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork, ILogger<CloseAccountCommandHandler> logger)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(CloseAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken)
                ?? throw new AccountNotFoundException();

            account.Close();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.Log(LogLevel.Information, "Account closed successfully");
        }
    }
}
