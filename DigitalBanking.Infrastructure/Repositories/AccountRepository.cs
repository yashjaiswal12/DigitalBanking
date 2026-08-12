using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DigitalBanking.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _context;

        public AccountRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAccountAsync(Account account, CancellationToken cancellationToken)
        {
            await _context.Accounts.AddAsync(account, cancellationToken);
        }

        public async Task<bool> ExistsByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken)
        {
            return await _context.Accounts.AnyAsync(x => x.AccountNumber == accountNumber, cancellationToken);
        }

        public async Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken)
        {
            return await _context.Accounts.SingleOrDefaultAsync(x => x.AccountNumber == accountNumber, cancellationToken);
        }

        public async Task<List<Account>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
        {
            return await _context.Accounts.Where(x => x.CustomerId == customerId).ToListAsync(cancellationToken);
        }

        public async Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Accounts.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
