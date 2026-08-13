using DigitalBanking.Application.Features.Customers.DTOs;
using MediatR;

namespace DigitalBanking.Application.Features.Customers.Queries.GetCustomerById
{
    public class GetCustomerByIdQuery : IRequest<CustomerDto>
    {
        public Guid Id { get; set; }
    }
}
