using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DigitalBanking.Infrastructure.Repositories
{
    public class BeneficiaryRepository : IBeneficiaryRepository
    {
        private readonly ApplicationDbContext _context;

        public BeneficiaryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddBeneficiaryAsync(Beneficiary beneficiary, CancellationToken cancellationToken)
        {
            await _context.Beneficiaries.AddAsync(beneficiary, cancellationToken);
        }

        public async Task<bool> BeneficiaryExistsAsync(Guid customerId, string beneficiaryAccountNumber, CancellationToken cancellationToken)
        {
            return await _context.Beneficiaries.AnyAsync(x => x.CustomerId == customerId && x.BeneficiaryAccountNumber == beneficiaryAccountNumber,
                cancellationToken);
        }

        public async Task<List<Beneficiary>> GetBeneficiariesByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken)
        {
            return await _context.Beneficiaries.Where(x => x.CustomerId == customerId).ToListAsync(cancellationToken);
        }

        public async Task<Beneficiary?> GetBeneficiaryByIdAsync(Guid beneficiaryId, Guid customerId, CancellationToken cancellationToken)
        {
            return await _context.Beneficiaries.SingleOrDefaultAsync(x => x.Id == beneficiaryId && x.CustomerId == customerId, cancellationToken);
        }

        public void RemoveBeneficiary(Beneficiary beneficiary)
        {
            _context.Beneficiaries.Remove(beneficiary);
        }
    }
}
