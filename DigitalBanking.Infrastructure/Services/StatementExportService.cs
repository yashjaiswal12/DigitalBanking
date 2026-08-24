using DigitalBanking.Application.Features.Statements.DTOs;
using DigitalBanking.Application.Features.Statements.Exports;
using System.Globalization;
using System.Text;

namespace DigitalBanking.Infrastructure.Services
{
    public class StatementExportService : IStatementExportService
    {
        public async Task<byte[]> GenerateAsync(AccountStatementDto statementDto, ExportFormat format, CancellationToken cancellationToken)
        {
            if (format != ExportFormat.CSV)
                throw new NotSupportedException("Export format is not supported");

            var csv = new StringBuilder();

            csv.AppendLine("TransactionId,ReferenceNumber,Date,Amount,Type,Status");

            foreach (var transaction in statementDto.Transactions)
            {
                csv.AppendLine(
                    string.Join(",",
                    transaction.TransactionId,
                    transaction.ReferenceNumber,
                    transaction.CompletedAtUtc.Value.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                    transaction.Amount,
                    transaction.Type,
                    transaction.Status)
                    );
            }

            csv.AppendLine();
            csv.AppendLine("Summary");
            csv.AppendLine($"Opening Balance {statementDto.OpeningBalance}");
            csv.AppendLine($"Closing Balance {statementDto.ClosingBalance}");
            csv.AppendLine($"Total Credits {statementDto.StatementSummary?.TotalCredits}");
            csv.AppendLine($"Total Debits {statementDto.StatementSummary?.TotalDebits}");

            return await Task.FromResult(Encoding.UTF8.GetBytes(csv.ToString()));
        }
    }
}
