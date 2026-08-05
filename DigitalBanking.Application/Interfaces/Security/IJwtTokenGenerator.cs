using DigitalBanking.Domain.Entities;

namespace DigitalBanking.Application.Interfaces.Security
{
    public interface IJwtTokenGenerator
    {
        string GenerateAccessToken(Customer customer);
        RefreshToken GenerateRefreshToken(Customer customer);
    }
}
