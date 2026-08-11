using DigitalBanking.Domain.Entities;

namespace DigitalBanking.Application.Interfaces.Persistence
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken);
        Task<List<Account>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);
        Task<bool> ExistsByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken);
        Task AddAccount(Account account, CancellationToken cancellationToken);
    }
}
