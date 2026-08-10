using FluentValidation;

namespace DigitalBanking.Application.Features.Customers.GetCustomerById
{
    public class GetCustomerByIdQueryValidator : AbstractValidator<GetCustomerByIdQuery>
    {
        public GetCustomerByIdQueryValidator() 
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
