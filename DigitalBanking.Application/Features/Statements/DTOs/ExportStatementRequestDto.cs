using DigitalBanking.Application.Features.Statements.Exports;

namespace DigitalBanking.Application.Features.Statements.DTOs
{
    public class ExportStatementRequestDto
    {
        public DateTime ToDateUtc { get; init; }
        public DateTime FromDateUtc { get; init; }
        public ExportFormat Format { get; init; }
    }
}
