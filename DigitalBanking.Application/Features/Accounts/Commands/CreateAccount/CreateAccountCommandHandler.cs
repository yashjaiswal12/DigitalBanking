using DigitalBanking.Application.Interfaces.Common;
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
        private readonly ICurrentUserService _userService;

        public CreateAccountCommandHandler(ICustomerRepository customerRepository, IAccountRepository accountRepository, IUnitOfWork unitOfWork,
            IAccountNumberGenerator accountNumberGenerator, ILogger<CreateAccountCommandHandler> logger, ICurrentUserService userService)
        {
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _unitOfWork = unitOfWork;
            _accountNumberGenerator = accountNumberGenerator;
            _logger = logger;
            _userService = userService;
        }

        public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(request.CustomerId, cancellationToken)
                ?? throw new CustomerNotFoundException();

            if (!customer.IsActive)
                throw new InvalidCustomerException("Inactive customers cannot create an account");

            var accountNumber = await _accountNumberGenerator.GenerateAsync(cancellationToken);
            var account = Account.Create(accountNumber, request.CustomerId, request.Type, request.Currency, 
                request.InitialBalance, _userService.UserId ?? null);

            await _accountRepository.AddAccountAsync(account, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.Log(LogLevel.Information, "Account created successfully");

            return account.Id;
        } 
    }
}
