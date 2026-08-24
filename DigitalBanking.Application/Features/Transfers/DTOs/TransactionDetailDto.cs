using DigitalBanking.Domain.Enums;

namespace DigitalBanking.Application.Features.Transfers.DTOs
{
    public class TransactionDetailDto
    {
        public Guid TransactionId { get; init; }
        public string ReferenceNumber { get; init; } = string.Empty;
        public Guid SourceAccountId { get; init; }
        public Guid DestinationAccountId { get; init; }
        public decimal Amount { get; init; }
        public string Type { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
        public DateTime? CompletedAtUtc { get; init; }
        public DateTime? CreatedAtUtc { get; init; }
        public string? FailureReason { get; init; }
    }
}
