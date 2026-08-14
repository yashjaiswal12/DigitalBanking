using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Application.Interfaces.Security;
using DigitalBanking.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DigitalBanking.Application.Features.Authentication.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ILogger<LoginCommandHandler> _logger;

        public LoginCommandHandler(IUnitOfWork unitOfWork, IRefreshTokenRepository refreshTokenRepository, ICustomerRepository customerRepository,
            IJwtTokenGenerator jwtTokenGenerator, IPasswordHasher passwordHasher, ILogger<LoginCommandHandler> logger) 
        { 
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
            _customerRepository = customerRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = passwordHasher;
            _logger = logger;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var customerEmailExists = await _customerRepository.CustomerExistsByEmailAsync(request.Email, null, cancellationToken);
            if (!customerEmailExists)
                throw new InvalidCredentialsException();

            var customer = await _customerRepository.GetCustomerByEmailAsync(request.Email, cancellationToken);

            if (customer.IsLocked)
                throw new AccountLockedException("Account is locked. Try again after sometime.");
            
            bool isValidPassword = _passwordHasher.Verify(request.Password, customer.PasswordHash);
            if (!isValidPassword)
            {
                customer.RecordFailedLogin();

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                throw new InvalidCredentialsException();
            }

            if (!customer.IsActive)
                throw new DomainException("Customer is InActive");

            var accessToken = _jwtTokenGenerator.GenerateAccessToken(customer);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(customer);

            var existingRefreshToken = await _refreshTokenRepository.GetRefreshTokenByCustomerIdAsync(customer.Id, cancellationToken);
            if (existingRefreshToken == null)
                await _refreshTokenRepository.AddTokenAsync(refreshToken, cancellationToken);
            else
                existingRefreshToken.UpdateRefreshToken(refreshToken.Token, refreshToken.ExpiresOn);

            customer.RecordSuccessfulLogin();

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Customer with an email {request.Email} logged in successfully");

            return new LoginResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = refreshToken.ExpiresOn
            };
        }
    }
}
