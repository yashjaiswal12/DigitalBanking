using DigitalBanking.Application.Features.Customers.DTOs;
using DigitalBanking.Application.Interfaces.Persistence;
using DigitalBanking.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DigitalBanking.Application.Features.Customers.Queries.SearchCustomers
{
    public class SearchCustomerQueryHandler : IRequestHandler<SearchCustomerQuery, List<CustomerDto>>
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<SearchCustomerQueryHandler> _logger;

        public SearchCustomerQueryHandler(ICustomerRepository customerRepository, ILogger<SearchCustomerQueryHandler> logger)
        {
            _customerRepository = customerRepository;
            _logger = logger;
        }

        public async Task<List<CustomerDto>> Handle(SearchCustomerQuery request, CancellationToken cancellationToken)
        {
            var customers = await _customerRepository.SearchCustomerAsync(request.SearchTerm, request.IsActive, cancellationToken);

            if (customers.Count == 0)
                throw new CustomerNotFoundException();

            _logger.Log(LogLevel.Information, "Retrieved relevant customers list based on the search term");

            return customers.Select(customer => new CustomerDto() 
            { 
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                RowVersion = Convert.ToBase64String(customer.RowVersion)
            }).ToList();
        }
    }
}
