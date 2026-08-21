using DigitalBanking.Domain.Enums;

namespace DigitalBanking.Application.Features.Transfers.DTOs
{
    public class TransactionHistoryDto
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
