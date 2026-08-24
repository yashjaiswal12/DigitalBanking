using DigitalBanking.Application.Features.Statements.DTOs;
using DigitalBanking.Application.Features.Statements.Exports;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Exceptions;
using MediatR;

namespace DigitalBanking.Application.Features.Statements.Queries.ExportStatement
{
    public class ExportStatementQueryHandler : IRequestHandler<ExportStatementQuery, FileExportDto>
    {
        private readonly IStatementQueries _statementQueries;
        private readonly IStatementExportService _statementExportService;

        public ExportStatementQueryHandler(IStatementExportService statementExportService, IStatementQueries statementQueries)
        {
            _statementQueries = statementQueries;
            _statementExportService = statementExportService;
        }

        public async Task<FileExportDto> Handle(ExportStatementQuery request, CancellationToken cancellationToken)
        {
            var accountStatements = await _statementQueries.GenerateAsync(request.AccountId, request.FromDateUtc, request.ToDateUtc, cancellationToken)
                ?? throw new DomainException("Unable to fetch account statements");

            var content = await _statementExportService.GenerateAsync(accountStatements, request.Format, cancellationToken);

            return new FileExportDto
            {
                Content = content,
                ContentType = "text/csv",
                FileName = $"statement-{request.AccountId:N}.csv"
            };
        }
    }
}
