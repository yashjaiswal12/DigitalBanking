using DigitalBanking.Application.Features.Customers.DTOs;
using DigitalBanking.Application.Interfaces.Common;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DigitalBanking.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, Customer>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateCustomerCommandHandler> _logger;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, 
            ILogger<UpdateCustomerCommandHandler> logger, IDateTimeProvider dateTimeProvider)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Customer> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdUpdateAsync(request.CustomerId, cancellationToken)
                ?? throw new CustomerNotFoundException();

            var normalizedEmail = request.Email.Trim().ToLowerInvariant();
            var customerExists = await _customerRepository.CustomerExistsByEmailAsync(normalizedEmail, cancellationToken);
            if (customerExists)
                throw new CustomerAlreadyExistsException(request.Email);

            customer.UpdateProfile(request.FirstName, request.LastName, request.Email, request.Phone);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new Customer
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Id = customer.Id
            };
        }
    }
}
