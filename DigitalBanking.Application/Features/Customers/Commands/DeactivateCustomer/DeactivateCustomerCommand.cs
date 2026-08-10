using MediatR;

namespace DigitalBanking.Application.Features.Customers.Commands.DeactivateCustomer
{
    public class DeactivateCustomerCommand : IRequest
    {
        public Guid CustomerId { get; set; }
    }
}
