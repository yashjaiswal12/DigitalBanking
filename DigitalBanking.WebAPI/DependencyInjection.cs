using DigitalBanking.Infrastructure;
using DigitalBanking.Application;

namespace DigitalBanking.WebAPI
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureApiDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddInrastructureDI(configuration);
            services.AddApplicationDI();
            return services;
        }
    }
}
