using MediatR;

namespace DigitalBanking.Application.Features.Accounts.Commands.FreezeAccount
{
    public class FreezeAccountCommand : IRequest
    {
        public Guid AccountId { get; set; }
    }
}
