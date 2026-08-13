using MediatR;

namespace DigitalBanking.Application.Features.Accounts.Commands.ActivateAccount
{
    public class ActivateAccountCommand : IRequest
    {
        public Guid AccountId { get; set; }
    }
}
