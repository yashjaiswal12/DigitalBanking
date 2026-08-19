namespace DigitalBanking.Application.Interfaces.Services
{
    public interface IReferenceNumberGenerator
    {
        public Task<string> GenerateAsync(CancellationToken cancellationToken);
    }
}
