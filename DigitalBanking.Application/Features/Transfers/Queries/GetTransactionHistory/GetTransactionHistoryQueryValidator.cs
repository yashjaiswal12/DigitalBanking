using DigitalBanking.Application.Common.Pagination;
using FluentValidation;

namespace DigitalBanking.Application.Features.Transfers.Queries.GetTransactionHistory
{
    public class GetTransactionHistoryQueryValidator : AbstractValidator<GetTransactionHistoryQuery>
    {
        public GetTransactionHistoryQueryValidator()
        {
            RuleFor(x => x.AccountId).NotEmpty();
            
            RuleFor(x => x.Status).IsInEnum();
            RuleFor(x => x.Type).IsInEnum();
            RuleFor(x => x.SortBy).IsInEnum();
            
            RuleFor(x => x.Page).GreaterThanOrEqualTo(PaginationConstants.DefaultPage);
            RuleFor(x => x.PageSize).LessThanOrEqualTo(PaginationConstants.MaxPageSize);
            
            RuleFor(x => x.MinAmount).GreaterThanOrEqualTo(1).When(x => x.MinAmount.HasValue);
            RuleFor(x => x.MaxAmount).GreaterThanOrEqualTo(1).When(x => x.MaxAmount.HasValue);
            
            RuleFor(x => x).Must(x => !x.FromDateUtc.HasValue || !x.ToDateUtc.HasValue || x.FromDateUtc <= x.ToDateUtc).
                WithMessage("FromDate must be less than or equal to ToDate");
            
            RuleFor(x => x).Must(x => !x.MinAmount.HasValue || !x.MaxAmount.HasValue || x.MinAmount <= x.MaxAmount).
                WithMessage("MinAmount must be less than or equal to MaxAmount");

            RuleFor(x => x.Search).MaximumLength(100).When(x => !string.IsNullOrWhiteSpace(x.Search));
        }
    }
}
