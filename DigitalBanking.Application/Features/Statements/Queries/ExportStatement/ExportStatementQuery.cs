using DigitalBanking.Application.Features.Statements.DTOs;
using DigitalBanking.Application.Features.Statements.Exports;
using MediatR;

namespace DigitalBanking.Application.Features.Statements.Queries.ExportStatement
{
    public class ExportStatementQuery : IRequest<FileExportDto>
    {
        public Guid AccountId { get; init; }
        public DateTime FromDateUtc { get; init; }
        public DateTime ToDateUtc { get; init; }
        public ExportFormat Format { get; init; }
    }
}
