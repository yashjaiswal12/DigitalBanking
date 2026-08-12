using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DigitalBanking.Application.Features.Accounts.Commands.FreezeAccount
{
    public class FreezeAccountCommandHandler : IRequestHandler<FreezeAccountCommand>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FreezeAccountCommandHandler> _logger;

        public FreezeAccountCommandHandler(IAccountRepository accountRepository, IUnitOfWork unitOfWork, ILogger<FreezeAccountCommandHandler> logger)
        {
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(FreezeAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.GetByIdAsync(request.AccountId, cancellationToken)
                ?? throw new AccountNotFoundException();

            account.Freeze();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.Log(LogLevel.Information, "Account froze");
        }
    }
}
