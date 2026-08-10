using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DigitalBanking.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        }

        public async Task<RefreshToken?> GetByRefreshTokenAsync(string token, CancellationToken cancellationToken)
        {
            return await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token == token, cancellationToken);
        }

        public async Task<RefreshToken?> GetRefreshTokenByCustomerIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.CustomerId == id, cancellationToken);
        }

        public async Task<bool> RefreshTokenByCustomerIdExistsAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.RefreshTokens.AnyAsync(x => x.CustomerId == id, cancellationToken);
        }

        public Task UpdateTokenAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            return Task.CompletedTask;
        }
    }
}
