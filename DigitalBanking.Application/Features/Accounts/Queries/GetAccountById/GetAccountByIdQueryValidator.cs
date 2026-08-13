using FluentValidation;

namespace DigitalBanking.Application.Features.Accounts.Queries.GetAccountById
{
    public class GetAccountByIdQueryValidator : AbstractValidator<GetAccountByIdQuery>
    {
        public GetAccountByIdQueryValidator()
        {
            RuleFor(x => x.AccountId).NotEmpty();
        }
    }
}
