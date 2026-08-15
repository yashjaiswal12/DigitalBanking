namespace DigitalBanking.Application.Interfaces.Common
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        bool? IsAuthenticated { get; }
        string? CustomerId { get; }
    }
}
