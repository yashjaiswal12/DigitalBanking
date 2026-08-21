namespace DigitalBanking.Application.Features.Statements.DTOs
{
    public class AccountStatementDto
    {
        public Guid AccountId { get; set; }
        public string? AccountNumber { get; set; }
        public string? CustomerName { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal ClosingBalance { get; set; }
        public DateTime? FromDateUtc { get; set; }
        public DateTime? ToDateTimeUtc { get; set; }
        public StatementSummaryDto? StatementSummary { get; set; }
        public IReadOnlyCollection<StatementTransactionDto> Transactions { get; set; } = [];
    }
}
