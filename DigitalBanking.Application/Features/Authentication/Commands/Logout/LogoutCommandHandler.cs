using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DigitalBanking.Application.Features.Authentication.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LogoutCommandHandler> _logger;

        public LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork, ILogger<LogoutCommandHandler> logger)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshTokenRepository.GetByRefreshTokenAsync(request.RefreshToken, cancellationToken) ??
                throw new InvalidTokenException("Refresh token is invalid");

            if (refreshToken.IsRevoked)
                throw new InvalidTokenException("Refresh token has already been revoked.");

            refreshToken.Revoke(DateTime.UtcNow);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Customer logged out successfully");
        }
    }
}
