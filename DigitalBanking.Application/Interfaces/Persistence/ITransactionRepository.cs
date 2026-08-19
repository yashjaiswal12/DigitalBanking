using DigitalBanking.Domain.Entities;

namespace DigitalBanking.Application.Interfaces.Persistence
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction, CancellationToken cancellationToken);
        Task<Transaction?> GetByTransactionIdAsync(Guid transactionId, CancellationToken cancellationToken);
        Task<Transaction?> GetByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken);
        Task<bool> ExistsByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken);
    }
}
