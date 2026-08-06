using FluentValidation;

namespace DigitalBanking.Application.Features.Authentication.Commands.Logout
{
    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator() 
        {
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}
