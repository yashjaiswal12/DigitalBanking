using DigitalBanking.Application;
using DigitalBanking.Infrastructure;
using DigitalBanking.WebAPI.Middlewares;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Threading.RateLimiting;

namespace DigitalBanking.WebAPI
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureApiDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                options.OnRejected = async (context, cancellationToken) =>
                {
                    await context.HttpContext.Response.WriteAsJsonAsync(
                        new { message = "Try after some time. Too many requests" }, 
                        cancellationToken);
                };

                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext,string>(context =>
                {
                    var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                    return RateLimitPartition.GetFixedWindowLimiter(partitionKey: ip, factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100,
                        QueueLimit = 0,
                        Window = TimeSpan.FromMinutes(1)
                    });
                });

                options.AddPolicy("request-limit", context =>
                {
                    var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                    return RateLimitPartition.GetFixedWindowLimiter(partitionKey: ip, factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 1000,
                        QueueLimit = 0,
                        Window = TimeSpan.FromMinutes(1)
                    });
                });

                options.AddFixedWindowLimiter("login-window", fixedWindow =>
                {
                    fixedWindow.PermitLimit = 5;
                    fixedWindow.QueueLimit = 0;
                    fixedWindow.Window = TimeSpan.FromMinutes(1);
                });
            });

            services.AddHealthChecks();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Digital Banking API",
                    Version = "v1",
                    Description = "Enterprise Digital Banking Platform built using ASP.NET Core, Clean Architecture, CQRS, MediatR and Azure."
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter: Bearer {your JWT token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.AddEndpointsApiExplorer();

            services.AddInrastructureDI(configuration);
            services.AddApplicationDI();

            return services;
        }
    }
}
