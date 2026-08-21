namespace DigitalBanking.Application.Features.Statements.DTOs
{
    public class StatementSummaryDto
    {
        public decimal TotalCredits { get; init; }
        public decimal TotalDebits { get; init; }
        public int TotalTransactions { get; init; }
    }
}
