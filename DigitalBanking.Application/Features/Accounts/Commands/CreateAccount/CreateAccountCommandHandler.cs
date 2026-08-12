using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Application.Interfaces.Services;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DigitalBanking.Application.Features.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Guid>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<CreateAccountCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAccountNumberGenerator _accountNumberGenerator;

        public CreateAccountCommandHandler(ICustomerRepository customerRepository, IAccountRepository accountRepository, IUnitOfWork unitOfWork,
            IAccountNumberGenerator accountNumberGenerator, ILogger<CreateAccountCommandHandler> logger)
        {
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _accountNumberGenerator = accountNumberGenerator;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(request.CustomerId, cancellationToken)
                ?? throw new CustomerNotFoundException();

            var accountNumber = await _accountNumberGenerator.GenerateAsync(cancellationToken);
            var account = Account.Create(accountNumber, request.CustomerId, request.Type, request.Currency, 
                request.InitialBalance, customer.Id.ToString());

            await _accountRepository.AddAccountAsync(account, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.Log(LogLevel.Information, "Account created successfully");

            return account.Id;
        } 
    }
}
