using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Application.Interfaces.Services;

namespace DigitalBanking.Infrastructure.Services
{
    public class ReferenceNumberGenerator : IReferenceNumberGenerator
    {
        private readonly ITransactionRepository _transactionRepository;

        public ReferenceNumberGenerator(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<string> GenerateAsync(CancellationToken cancellationToken)
        {
            int attempts = 0;
            do
            {
                var referenceNumber = GenerateReferenceNumber();
                var existsReferenceNumber = await _transactionRepository.ExistsByReferenceNumberAsync(referenceNumber, cancellationToken);

                if (!existsReferenceNumber)
                    return referenceNumber;

                attempts++;
            } while (attempts < 10);

            throw new InvalidOperationException("Unable to generate an unique reference number");
        }

        private string GenerateReferenceNumber()
        {
            var random = Random.Shared.NextInt64(100_00, 1_000_00);
            return $"TXN{DateTime.UtcNow.ToString("ddmmyyyy")}{random}";
        }
    }
}
