using DigitalBanking.Application.Features.Statements.DTOs;

namespace DigitalBanking.Application.Features.Statements.Exports
{
    public interface IStatementExportService
    {
        Task<byte[]> GenerateAsync(AccountStatementDto statementDto, ExportFormat format, CancellationToken cancellationToken);
    }
}
