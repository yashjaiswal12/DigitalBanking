using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Application.Interfaces.Services;

namespace DigitalBanking.Infrastructure.Services
{
    public sealed class AccountNumberGenerator : IAccountNumberGenerator
    {
        private readonly IAccountRepository _accountRepository;

        public AccountNumberGenerator(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<string> GenerateAsync(CancellationToken cancellationToken)
        {
            var attempts = 0;
            do
            {
                var accountNumber = GenerateAccountNumber();

                var accountNumberExists = await _accountRepository.ExistsByAccountNumberAsync(accountNumber, cancellationToken);
                if (!accountNumberExists)
                    return accountNumber;

                attempts++;
            } while (attempts <= 10);

            throw new InvalidOperationException("Unable to generate an unique account number");
        }

        private static string GenerateAccountNumber()
        {
            var random = Random.Shared.NextInt64(100_000_000_000, 1_000_000_000_000);
            return random.ToString();
        }
    }
}
