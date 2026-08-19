using DigitalBanking.Application.Features.Transfers.DTOs;
using MediatR;

namespace DigitalBanking.Application.Features.Transfers.Commands
{
    public class TransferFundsCommand : IRequest<TransferFundsDto>
    {
        public Guid SourceAccountId { get; set; }
        public Guid DestinationAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
