using DigitalBanking.Domain.Enums;
using MediatR;

namespace DigitalBanking.Application.Features.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommand : IRequest<Guid>
    {
        public Guid CustomerId { get; set; }
        public string Currency { get; set; } = string.Empty;
        public AccountType Type { get; set; }
        public decimal InitialBalance { get; set; }
    }
}
