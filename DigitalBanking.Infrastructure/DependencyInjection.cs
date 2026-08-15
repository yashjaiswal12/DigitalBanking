using DigitalBanking.Application.Authorization;
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

                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = async context =>
                        {
                            var validator = context.HttpContext.RequestServices.GetRequiredService<JwtTokenEvents>();
                            await validator.ValidateJwtToken(context);
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(Permissions.ManageAccounts, policy => policy.RequireClaim("permission", Permissions.ManageAccounts));
                options.AddPolicy(Permissions.ViewAccounts, policy => policy.RequireClaim("permission", Permissions.ViewAccounts));
                options.AddPolicy(Permissions.FreezeAccounts, policy => policy.RequireClaim("permission", Permissions.FreezeAccounts));

                options.AddPolicy(Permissions.ViewAuditLogs, policy => policy.RequireClaim("permission", Permissions.ViewAuditLogs));

                options.AddPolicy(Permissions.ManageCustomers, policy => policy.RequireClaim("permission", Permissions.ManageCustomers));
                options.AddPolicy(Permissions.ViewCustomers, policy => policy.RequireClaim("permission", Permissions.ViewCustomers));

                options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            });

            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountQueries, AccountQueries>();
            services.AddScoped<JwtTokenEvents>();
            services.AddScoped<IBeneficiaryRepository, BeneficiaryRepository>();

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
