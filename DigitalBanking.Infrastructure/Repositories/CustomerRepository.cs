using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DigitalBanking.Infrastructure.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddCustomerAsync(Customer customer, CancellationToken cancellationToken)
        {
            await _context.Customers.AddAsync(customer, cancellationToken);
        }

        public async Task<bool> CustomerExistsByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Customers.AnyAsync(x => x.Email.Equals(email), cancellationToken);
        }

        public async Task<bool> CustomerExistsByPhoneAsync(string phone, CancellationToken cancellationToken)
        {
            return await _context.Customers.AnyAsync(x => x.PhoneNumber == phone, cancellationToken);
        }

        public void DeleteCustomer(Customer customer)
        {
            _context.Customers.Remove(customer);
        }

        public async Task<Customer?> GetByIdUpdateAsync(Guid customerId, CancellationToken cancellationToken)
        {
            return await _context.Customers.SingleOrDefaultAsync(x => x.Id == customerId, cancellationToken);
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Customers.AsNoTracking().SingleOrDefaultAsync(x => x.Email.Equals(email), cancellationToken);
        }

        public async Task<Customer?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Customers.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<List<Customer>> SearchCustomerAsync(string searchTerm, bool? isActive, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.AsNoTracking()
                .Where(x => (x.FirstName.Contains(searchTerm) || x.LastName.Contains(searchTerm) || x.Email.Contains(searchTerm) 
                || x.PhoneNumber.Contains(searchTerm)) && (isActive == null || x.IsActive == isActive))
                .OrderBy(x => x.CreatedAtUtc)
                .ToListAsync(cancellationToken);

            return customer;
        }

        public void UpdateCustomer(Customer customer)
        {
            _context.Customers.Update(customer);
        }
    }
}
