using DigitalBanking.Domain.Entities;

namespace DigitalBanking.Application.Interfaces.Persistence
{
    public interface IRefreshTokenRepository
    {
        Task AddTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
        Task<RefreshToken?> GetByRefreshTokenAsync(string token, CancellationToken cancellationToken);
        Task UpdateTokenAsync(RefreshToken refreshToken);
        Task<bool> RefreshTokenByCustomerIdExistsAsync(Guid id, CancellationToken cancellationToken);
        Task<RefreshToken?> GetRefreshTokenByCustomerIdAsync(Guid id, CancellationToken cancellationToken);
        Task<List<RefreshToken>> GetRefreshTokensByCustomerIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
