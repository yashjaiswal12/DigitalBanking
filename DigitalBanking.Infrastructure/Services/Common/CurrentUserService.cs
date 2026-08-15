using DigitalBanking.Application.Interfaces.Common;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DigitalBanking.Infrastructure.Services.Common
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid UserId => Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out Guid customerId)
            ? customerId : throw new InvalidDataException("Customer id is invalid");

        public bool? IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated;

        public string? CustomerId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
