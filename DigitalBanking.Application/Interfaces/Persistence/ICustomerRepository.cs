using DigitalBanking.Domain.Entities;

namespace DigitalBanking.Application.Interfaces.Persistence
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<Customer?> GetCustomerByEmailAsync(string email, CancellationToken cancellationToken);
        Task AddCustomerAsync(Customer customer, CancellationToken cancellationToken);
        Task<bool> CustomerExistsByEmailAsync(string email, Guid? excludingCustomerId, CancellationToken cancellationToken);
        Task<bool> CustomerExistsByPhoneAsync(string phone, Guid? excludingCustomerId, CancellationToken cancellationToken);
        Task<bool> CustomerExistsByPhoneAsync(string phone, CancellationToken cancellationToken);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Customer customer);
        Task<List<Customer>> SearchCustomerAsync(string searchTerm, bool? isActive, CancellationToken cancellationToken);
        Task<Customer?> GetByIdUpdateAsync(Guid customerId, CancellationToken cancellationToken);
    }
}
