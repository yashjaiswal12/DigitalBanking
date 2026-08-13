namespace DigitalBanking.Application.Interfaces.Services
{
    public interface IAccountNumberGenerator
    {
        Task<string> GenerateAsync(CancellationToken cancellationToken);
    }
}
