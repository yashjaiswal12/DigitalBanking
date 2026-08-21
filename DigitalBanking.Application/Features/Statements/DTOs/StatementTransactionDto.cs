namespace DigitalBanking.Application.Features.Statements.DTOs
{
    public class StatementTransactionDto
    {
        public Guid TransactionId { get; init; }
        public Guid SourceAccountId { get; init; }
        public Guid DestinationAccountId { get; init; }
        public string ReferenceNumber { get; init; } = string.Empty;
        public string Type { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public decimal Amount { get; init; }
        public DateTime? CreatedAtUtc { get; init; }
        public DateTime? CompletedAtUtc { get; init; }
    }
}
