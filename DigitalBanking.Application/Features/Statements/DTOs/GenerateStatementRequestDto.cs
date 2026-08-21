namespace DigitalBanking.Application.Features.Statements.DTOs
{
    public class GenerateStatementRequestDto
    {
        public DateTime FromDateUtc { get; init; }
        public DateTime ToDateUtc { get; init; }
    }
}
