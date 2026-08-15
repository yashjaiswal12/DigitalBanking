using DigitalBanking.Application.Interfaces.Common;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Exceptions;
using MediatR;

namespace DigitalBanking.Application.Features.Authentication.Commands.LogoutAllDevices
{
    public class LogoutAllDevicesCommandHandler : IRequestHandler<LogoutAllDevicesCommand>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ICurrentUserService _currentUserService;

        public LogoutAllDevicesCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork, 
            IRefreshTokenRepository refreshTokenRepository, IDateTimeProvider dateTimeProvider, ICurrentUserService currentUserService)
        {
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
            _dateTimeProvider = dateTimeProvider;
            _currentUserService = currentUserService;
        }

        public async Task Handle(LogoutAllDevicesCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdUpdateAsync(_currentUserService.UserId, cancellationToken)
                ?? throw new CustomerNotFoundException();

            customer.UpdateTokenVersion();
            customer.UpdateSecurityStamp();

            var refreshTokens = await _refreshTokenRepository.GetRefreshTokensByCustomerIdAsync(customer.Id, cancellationToken);
            foreach (var refreshToken in refreshTokens)
            {
                if (!refreshToken.IsRevoked)
                    refreshToken.Revoke(_dateTimeProvider.UtcNow);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
