using FluentValidation;

namespace DigitalBanking.Application.Features.Statements.Queries.GetStatement
{
    public class GetStatementQueryValidator : AbstractValidator<GetStatementQuery>
    {
        public GetStatementQueryValidator()
        {
            RuleFor(x => x.AccountId).NotEmpty().WithMessage("AccountId is required field");
            RuleFor(x => x.FromDateUtc).NotEmpty();
            RuleFor(x => x.ToDateUtc).NotEmpty();
            RuleFor(x => x).Must(x => x.FromDateUtc <= x.ToDateUtc).WithMessage("FromDate must be less than or equal to ToDate");
            RuleFor(x => x).Must(x => x.ToDateUtc <= x.FromDateUtc.AddMonths(12));
        }
    }
}
