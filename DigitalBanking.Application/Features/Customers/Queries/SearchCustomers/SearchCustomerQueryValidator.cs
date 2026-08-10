using FluentValidation;

namespace DigitalBanking.Application.Features.Customers.Queries.SearchCustomers
{
    public class SearchCustomerQueryValidator : AbstractValidator<SearchCustomerQuery>
    {
        public SearchCustomerQueryValidator()
        {
            RuleFor(x => x.SearchTerm).NotEmpty().WithMessage("Search term is required")
                .MinimumLength(2).WithMessage("Search term must contain atleast contain 2 characters")
                .MaximumLength(100).WithMessage("Search tern should not contain more than 100 characters");
        }
    }
}
