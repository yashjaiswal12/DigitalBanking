using MediatR;

namespace DigitalBanking.Application.Features.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<RefreshTokenResponse>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
