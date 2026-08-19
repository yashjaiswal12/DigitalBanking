using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DigitalBanking.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Transaction transaction, CancellationToken cancellationToken)
        {
            await _context.Transactions.AddAsync(transaction, cancellationToken);
        }

        public async Task<bool> ExistsByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken)
        {
            return await _context.Transactions.AnyAsync(x => x.ReferenceNumber == referenceNumber, cancellationToken);
        }

        public async Task<Transaction?> GetByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken)
        {
            return await _context.Transactions.SingleOrDefaultAsync(x => x.ReferenceNumber == referenceNumber, cancellationToken);
        }

        public async Task<Transaction?> GetByTransactionIdAsync(Guid transactionId, CancellationToken cancellationToken)
        {
            return await _context.Transactions.SingleOrDefaultAsync(x => x.Id == transactionId, cancellationToken);
        }
    }
}
