using DigitalBanking.Application.Features.Customers.DTOs;
using MediatR;

namespace DigitalBanking.Application.Features.Customers.GetCustomerById
{
    public class GetCustomerByIdQuery : IRequest<Customer>
    {
        public Guid Id { get; set; }
    }
}
