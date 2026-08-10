using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Runtime.CompilerServices;

namespace DigitalBanking.Application.Features.Customers.Commands.DeactivateCustomer
{
    public class DeactivateCustomerCommandHandler : IRequestHandler<DeactivateCustomerCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<DeactivateCustomerCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public DeactivateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, 
            ILogger<DeactivateCustomerCommandHandler> logger, IRefreshTokenRepository refreshTokenRepository)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task Handle(DeactivateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdUpdateAsync(request.CustomerId, cancellationToken)
                ?? throw new CustomerNotFoundException();

            customer.Deactivate();
            customer.Delete();

            var refreshToken = await _refreshTokenRepository.GetRefreshTokenByCustomerIdAsync(customer.Id, cancellationToken);
            if (refreshToken != null)
                refreshToken.Revoke(DateTime.UtcNow);

            _customerRepository.DeleteCustomer(customer);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
