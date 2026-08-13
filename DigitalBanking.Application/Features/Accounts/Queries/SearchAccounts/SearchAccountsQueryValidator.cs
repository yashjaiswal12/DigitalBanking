using FluentValidation;

namespace DigitalBanking.Application.Features.Accounts.Queries.SearchAccounts
{
    public class SearchAccountsQueryValidator : AbstractValidator<SearchAccountsQuery>
    {
        public SearchAccountsQueryValidator()
        {
            RuleFor(x => x.AccountNumber).MaximumLength(50).When(x => !string.IsNullOrWhiteSpace(x.AccountNumber));
            RuleFor(x => x.Currency).MaximumLength(3).When(x => !string.IsNullOrWhiteSpace(x.Currency));
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }
}
