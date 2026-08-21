using DigitalBanking.Application.Features.Statements.DTOs;

namespace DigitalBanking.Application.Interfaces.Persistence
{
    public interface IStatementQueries
    {
        Task<AccountStatementDto> GenerateAsync(Guid accountId, DateTime fromDateUtc, DateTime toDateUtc, CancellationToken cancellationToken);
    }
}
