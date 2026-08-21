namespace DigitalBanking.Application.Features.Transfers.DTOs
{
    public class TransactionHistoryFilterDto
    {
        public DateTime? FromDateUtc { get; init; }
        public DateTime? ToDateUtc { get; init; }
        public decimal? MinAmount { get; init; }
        public decimal? MaxAmount { get; init; }
        public string? Search { get; init; }
        public string? TransactionType { get; init; }
        public string? TransactionStatus { get; init; }
    }
}
