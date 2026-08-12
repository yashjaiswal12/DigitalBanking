using MediatR;

namespace DigitalBanking.Application.Features.Accounts.Commands.CloseAccount
{
    public class CloseAccountCommand : IRequest
    {
        public Guid AccountId { get; set; }
    }
}
