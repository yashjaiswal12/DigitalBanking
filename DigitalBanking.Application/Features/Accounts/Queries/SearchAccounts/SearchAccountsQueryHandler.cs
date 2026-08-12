using DigitalBanking.Application.Features.Accounts.DTOs;
using DigitalBanking.Application.Interfaces.Persistence;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DigitalBanking.Application.Features.Accounts.Queries.SearchAccounts
{
    public class SearchAccountsQueryHandler : IRequestHandler<SearchAccountsQuery, List<Account>>
    {
        private readonly IAccountQueries _accountQueries;
        private readonly ILogger<SearchAccountsQueryHandler> _logger;

        public SearchAccountsQueryHandler(IAccountQueries accountQueries, ILogger<SearchAccountsQueryHandler> logger)
        {
            _accountQueries = accountQueries;
            _logger = logger;
        }

        public async Task<List<Account>> Handle(SearchAccountsQuery request, CancellationToken cancellationToken)
        {
            var searchAccountCriteria = new SearchAccountCriteria
            {
                AccountNumber = request.AccountNumber,
                CustomerId = request.CustomerId,
                Currency = request.Currency,
                Status = request.Status,
                Type = request.Type,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            return await _accountQueries.SearchAccountsAsync(searchAccountCriteria, cancellationToken);
        }
    }
}
