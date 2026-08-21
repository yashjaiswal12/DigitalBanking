using DigitalBanking.Application.Features.Transfers.DTOs;
using MediatR;

namespace DigitalBanking.Application.Features.Transfers.Queries.GetTransactionById
{
    public class GetTransactionByIdQuery : IRequest<TransactionDetailDto>
    {
        public Guid TransactionId { get; set; }
    }
}
