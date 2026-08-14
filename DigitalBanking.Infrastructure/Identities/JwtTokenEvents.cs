using DigitalBanking.Application.Interfaces.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace DigitalBanking.Infrastructure.Identities
{
    public class JwtTokenEvents
    {
        private readonly ICustomerRepository _customerRepository;

        public JwtTokenEvents(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task ValidateJwtToken(TokenValidatedContext context)
        {
            var customerIdClaim = context.Principal?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var securityStamp = context.Principal?.FindFirst("security_stamp")?.Value;
            var tokenVersion = context.Principal?.FindFirst("token_version")?.Value;

            if (!Guid.TryParse(customerIdClaim, out var customerId))
            {
                context.Fail("Invalid customer id");
                return;
            }

            if (string.IsNullOrWhiteSpace(securityStamp))
            {
                context.Fail("Security stamp is empty");
                return;
            }

            if (string.IsNullOrWhiteSpace(tokenVersion))
            {
                context.Fail("Token version is empty");
                return;
            }

            var customer = await _customerRepository.GetCustomerByIdAsync(customerId, context.HttpContext.RequestAborted);
            if (customer is null)
            {
                context.Fail("Customer not found");
                return;
            }

            if (securityStamp != customer.SecurityStamp)
            {
                context.Fail("Security stamp mismatch");
                return;
            }

            if (int.TryParse(tokenVersion, out int actualVersion) && actualVersion != customer.TokenVersion)
            {
                context.Fail("Token version is not correct");
                return;
            }
        }
    }
}
