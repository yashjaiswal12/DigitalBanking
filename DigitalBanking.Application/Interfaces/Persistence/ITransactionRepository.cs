using DigitalBanking.Domain.Entities;

namespace DigitalBanking.Application.Interfaces.Persistence
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction, CancellationToken cancellationToken);
        IQueryable<Transaction> GetQueryable();
        Task<Transaction?> GetByTransactionIdAsync(Guid transactionId, CancellationToken cancellationToken);
        Task<Transaction?> GetByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken);
        Task<bool> ExistsByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken);
        Task<Transaction?> GetPagedTransactionsAsync(CancellationToken cancellationToken);
        Task<Transaction?> GetTransactionByReferenceNumberAsync(string referenceNumber, CancellationToken cancellationToken);
    }
}
