using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Application.Interfaces.Security;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Domain.Exceptions;
using MediatR;

namespace DigitalBanking.Application.Features.Authentication.Commands.RegisterCustomer
{
    public class RegisterCustomerCommandHandler : IRequestHandler<RegisterCustomerCommand ,RegisterCustomerResponse>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterCustomerCommandHandler(ICustomerRepository customerRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<RegisterCustomerResponse> Handle(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var emailExists = await _customerRepository.CustomerExistsByEmailAsync(request.Email.Trim().ToLowerInvariant(), cancellationToken);
            if (emailExists)
                throw new CustomerAlreadyExistsException(request.Email);

            var passwordHash = _passwordHasher.Hash(request.Password);

            var customer = Customer.Create(request.FirstName, request.LastName, request.Email, request.PhoneNumber, passwordHash);
            await _customerRepository.AddCustomerAsync(customer, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RegisterCustomerResponse(customer.Id, "Customer Created Successfully!");
        }
    }
}
