namespace DigitalBanking.Application.Interfaces.Services
{
    public interface IDomainEventDispatcher
    {
        Task DispatchAsync(CancellationToken cancellationToken);
    }
}
