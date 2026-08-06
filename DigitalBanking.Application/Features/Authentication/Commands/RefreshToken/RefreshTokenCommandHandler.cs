using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Application.Interfaces.Security;
using DigitalBanking.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DigitalBanking.Application.Features.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RefreshTokenCommandHandler> _logger;

        public RefreshTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository, ICustomerRepository customerRepository,
            IJwtTokenGenerator jwtTokenGenerator, IUnitOfWork unitOfWork, ILogger<RefreshTokenCommandHandler> logger)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _customerRepository = customerRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var utcNow = DateTime.UtcNow;

            var refreshToken = await  _refreshTokenRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken) ??
                throw new InvalidTokenException("Token is invalid");

            if (refreshToken.IsRevoked)
                throw new InvalidTokenException("Refresh token has already been revoked.");

            if (refreshToken.IsExpired(utcNow))
                throw new InvalidTokenException("Refresh token has expired.");

            var customer = await _customerRepository.GetCustomerByIdAsync(refreshToken.CustomerId, cancellationToken) ??
                throw new CustomerNotFoundException();
                
            if (!customer.IsActive)
                throw new DomainException("Customer is inactive");

            refreshToken.Revoke(utcNow);

            var accessToken = _jwtTokenGenerator.GenerateAccessToken(customer);
            var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken(customer);

            // refresh token rotation
            await _refreshTokenRepository.UpdateTokenAsync(refreshToken);
            await _refreshTokenRepository.AddTokenAsync(newRefreshToken, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Refresh token rotation completed");

            return new RefreshTokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = newRefreshToken.Token,
                ExpiresAt = newRefreshToken.ExpiresOn
            };
        }
    }
}
