using DigitalBanking.Application.Interfaces.Common;

namespace DigitalBanking.Infrastructure.Services.Common
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
