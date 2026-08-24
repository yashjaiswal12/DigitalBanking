using DigitalBanking.Application.Features.Statements.DTOs;
using MediatR;

namespace DigitalBanking.Application.Features.Statements.Queries.GetStatement
{
    public class GetStatementQuery : IRequest<AccountStatementDto>
    {
        public Guid AccountId { get; init; }
        public DateTime FromDateUtc { get; init; }
        public DateTime ToDateUtc { get; init; }
    }
}
