using DigitalBanking.Application.Features.Statements.DTOs;
using DigitalBanking.Application.Interfaces.Persistence;
using MediatR;

namespace DigitalBanking.Application.Features.Statements.Queries.GetStatement
{
    public class GetStatementQueryHandler : IRequestHandler<GetStatementQuery, AccountStatementDto>
    {
        private readonly IStatementQueries _statementQueries;

        public GetStatementQueryHandler(IStatementQueries statementQueries)
        {
            _statementQueries = statementQueries;
        }

        public async Task<AccountStatementDto> Handle(GetStatementQuery request, CancellationToken cancellationToken)
        {
            return await _statementQueries.GenerateAsync(request.AccountId, request.FromDateUtc, request.ToDateUtc, cancellationToken);
        }
    }
}
