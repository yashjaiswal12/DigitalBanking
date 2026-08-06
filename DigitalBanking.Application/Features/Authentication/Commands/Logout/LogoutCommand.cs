using MediatR;

namespace DigitalBanking.Application.Features.Authentication.Commands.Logout
{
    public class LogoutCommand : IRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
