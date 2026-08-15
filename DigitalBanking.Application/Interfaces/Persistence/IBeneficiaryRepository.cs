using DigitalBanking.Domain.Entities;

namespace DigitalBanking.Application.Interfaces.Persistence
{
    public interface IBeneficiaryRepository
    {
        Task<List<Beneficiary>> GetBeneficiariesByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken);
        Task<Beneficiary?> GetBeneficiaryByIdAsync(Guid beneficiaryId, Guid customerId, CancellationToken cancellationToken);
        Task<bool> BeneficiaryExistsAsync(Guid customerId, string beneficiaryAccountNumber, CancellationToken cancellationToken);
        Task AddBeneficiaryAsync(Beneficiary beneficiary, CancellationToken cancellationToken);
        void RemoveBeneficiary(Beneficiary beneficiary);
    }
}
