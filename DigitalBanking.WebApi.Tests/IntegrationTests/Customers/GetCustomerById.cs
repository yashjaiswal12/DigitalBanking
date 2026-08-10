using DigitalBanking.Domain.Entities;
using DigitalBanking.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace DigitalBanking.WebApi.Tests.IntegrationTests.Customers
{
    public class GetCustomerById : WebApplicationFactory<Program>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public GetCustomerById(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        //private async Task SeedData(Guid customerId)
        //{
        //    using var scope = _factory.Services.CreateScope();
        //    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        //    var customer = Customer.Create("test", "test", "test", "test", "test");
        //    dbContext.Customers.Add();
        //}
    }
}
