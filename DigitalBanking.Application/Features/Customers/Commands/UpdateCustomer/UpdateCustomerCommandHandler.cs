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

        public UpdateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, 
            ILogger<UpdateCustomerCommandHandler> logger)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Customer> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdUpdateAsync(request.CustomerId, cancellationToken)
                ?? throw new CustomerNotFoundException();

            var normalizedEmail = request.Email.Trim().ToLowerInvariant();
            var customerEmailExists = await _customerRepository.CustomerExistsByEmailAsync(normalizedEmail, customer.Id, cancellationToken);
            if (customerEmailExists)
                throw new CustomerAlreadyExistsException(request.Email);


            var normalizedPhone = request.Phone.Replace(" ", "").Replace("+91", "").Replace("-", "");
            var customerPhoneExists = await _customerRepository.CustomerExistsByPhoneAsync(normalizedPhone, customer.Id, cancellationToken);
            if (customerPhoneExists)
                throw new CustomerAlreadyExistsException(request.Phone);

            customer.UpdateProfile(request.FirstName, request.LastName, request.Email, request.Phone);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.Log(LogLevel.Information, "Customer information with {id} updated sucessfully", request.CustomerId);

            return new Customer
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Id = customer.Id,
                RowVersion = Convert.ToBase64String(customer.RowVersion)
            };
        }
    }
}
