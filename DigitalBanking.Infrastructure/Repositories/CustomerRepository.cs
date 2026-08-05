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

        public async Task<Customer?> GetCustomerByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Customers.SingleOrDefaultAsync(x => x.Email.Equals(email), cancellationToken);
        }

        public async Task<Customer?> GetCustomerByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Customers.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
