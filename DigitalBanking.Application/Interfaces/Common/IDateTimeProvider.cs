namespace DigitalBanking.Application.Interfaces.Common
{
    public interface IDateTimeProvider
    {
        public DateTime UtcNow { get; }
    }
}
