using FluentValidation;

namespace DigitalBanking.Application.Features.Customers.Commands.DeactivateCustomer
{
    public class DeactivateCustomerCommandValidator : AbstractValidator<DeactivateCustomerCommand>
    {
        public DeactivateCustomerCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
        }
    }
}
