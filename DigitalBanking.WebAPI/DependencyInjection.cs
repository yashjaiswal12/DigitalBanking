using DigitalBanking.Application;
using DigitalBanking.Infrastructure;
using DigitalBanking.WebAPI.Middlewares;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace DigitalBanking.WebAPI
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureApiDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

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
