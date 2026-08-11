using DigitalBanking.Application.Interfaces.Common;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Application.Interfaces.Security;
using DigitalBanking.Application.Interfaces.Services;
using DigitalBanking.Infrastructure.Identities;
using DigitalBanking.Infrastructure.Identities.Configuration;
using DigitalBanking.Infrastructure.Persistence;
using DigitalBanking.Infrastructure.Repositories;
using DigitalBanking.Infrastructure.Services;
using DigitalBanking.Infrastructure.Services.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DigitalBanking.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]))
                    };
                });


            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

            services.AddScoped<IDateTimeProvider, DateTimeProvider>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IAccountNumberGenerator, AccountNumberGenerator>();
            
            return services;
        }
    }
}
