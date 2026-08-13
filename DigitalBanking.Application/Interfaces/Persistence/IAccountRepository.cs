using DigitalBanking.Domain.Entities;
using DigitalBanking.Domain.Enums;

namespace DigitalBanking.Application.Interfaces.Persistence
{
    public interface IAccountRepository
    {
        Task<Account?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken);
        Task<List<Account>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);
        Task<bool> ExistsByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken);
        Task AddAccountAsync(Account account, CancellationToken cancellationToken);
    }
}
