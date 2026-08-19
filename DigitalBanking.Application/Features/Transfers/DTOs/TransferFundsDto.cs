namespace DigitalBanking.Application.Features.Transfers.DTOs
{
    public class TransferFundsDto
    {
        public Guid TransactionId { get; init; }
        public string ReferenceNumber { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
    }
}
