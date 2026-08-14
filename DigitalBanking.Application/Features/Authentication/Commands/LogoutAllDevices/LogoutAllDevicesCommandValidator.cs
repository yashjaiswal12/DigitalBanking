using FluentValidation;

namespace DigitalBanking.Application.Features.Authentication.Commands.LogoutAllDevices
{
    public class LogoutAllDevicesCommandValidator : AbstractValidator<LogoutAllDevicesCommand>
    {
        public LogoutAllDevicesCommandValidator()
        {
        }
    }
}
