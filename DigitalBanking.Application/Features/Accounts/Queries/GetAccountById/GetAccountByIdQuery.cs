using DigitalBanking.Application.Features.Accounts.DTOs;
using MediatR;

namespace DigitalBanking.Application.Features.Accounts.Queries.GetAccountById
{
    public class GetAccountByIdQuery : IRequest<AccountDto>
    {
        public Guid AccountId { get; set; }
    }
}
