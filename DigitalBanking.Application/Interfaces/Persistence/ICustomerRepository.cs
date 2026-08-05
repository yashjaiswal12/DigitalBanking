using DigitalBanking.Domain.Entities;

namespace DigitalBanking.Application.Interfaces.Persistence
{
    public interface ICustomerRepository
    {
        Task<Customer> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Customer> GetCustomerByEmailAsync(string email, CancellationToken cancellationToken);
        Task AddCustomerAsync(Customer customer, CancellationToken cancellationToken);
        Task<bool> CustomerExistsByEmailAsync(string email, CancellationToken cancellationToken);
    }
}
