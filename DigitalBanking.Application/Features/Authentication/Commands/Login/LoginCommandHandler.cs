using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Application.Interfaces.Security;
using DigitalBanking.Domain.Exceptions;
using MediatR;

namespace DigitalBanking.Application.Features.Authentication.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IPasswordHasher _passwordHasher;

        public LoginCommandHandler(IUnitOfWork unitOfWork, IRefreshTokenRepository refreshTokenRepository, ICustomerRepository customerRepository,
            IJwtTokenGenerator jwtTokenGenerator, IPasswordHasher passwordHasher) 
        { 
            _unitOfWork = unitOfWork;
            _refreshTokenRepository = refreshTokenRepository;
            _customerRepository = customerRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = passwordHasher;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var customerExists = await _customerRepository.CustomerExistsByEmailAsync(request.Email, cancellationToken);
            if (!customerExists)
                throw new InvalidCustomerException(request.Email);

            var customer = await _customerRepository.GetCustomerByEmailAsync(request.Email, cancellationToken);
            
            bool isValidPassword = _passwordHasher.Verify(request.Password, customer.PasswordHash);
            if (!isValidPassword)
                throw new InvalidCustomerException(request.Email);

            if (!customer.IsActive)
                throw new DomainException("Customer is InActive");

            var accessToken = _jwtTokenGenerator.GenerateAccessToken(customer);
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(customer);

            var refreshTokenExists = await _refreshTokenRepository.GetRefreshTokenByCustomerIdAsync(customer.Id, cancellationToken);
            if (!refreshTokenExists)
                await _refreshTokenRepository.AddTokenAsync(refreshToken, cancellationToken);
            else
                await _refreshTokenRepository.UpdateTokenAsync(refreshToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new LoginResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = refreshToken.ExpiresOn
            };
        }
    }
}
