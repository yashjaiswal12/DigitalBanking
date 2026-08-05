using DigitalBanking.Application.Interfaces.Security;
using DigitalBanking.Domain.Entities;
using DigitalBanking.Infrastructure.Identities.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DigitalBanking.Infrastructure.Identities
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;

        public JwtTokenGenerator(IOptionsMonitor<JwtOptions> optionsMonitor)
        {
            _jwtOptions = optionsMonitor.CurrentValue;
        }

        public string GenerateAccessToken(Customer customer)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>() 
            { 
                new Claim(JwtRegisteredClaimNames.Sub, customer.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.GivenName, customer.FirstName),
                new Claim(JwtRegisteredClaimNames.FamilyName, customer.LastName),
                new Claim(JwtRegisteredClaimNames.Email, customer.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.AccessTokenExpiryMinutes),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken(Customer customer)
        {
            byte[] randomBytes = new byte[64];
            using var randomNum = RandomNumberGenerator.Create();
            randomNum.GetBytes(randomBytes);
            var token = Convert.ToBase64String(randomBytes);

            return RefreshToken.Create(customer.Id, token, DateTime.UtcNow.AddDays(7), DateTime.UtcNow);
        }
    }
}
