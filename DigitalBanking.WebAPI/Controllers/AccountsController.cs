using Asp.Versioning;
using DigitalBanking.Application.Features.Accounts.Commands.ActivateAccount;
using DigitalBanking.Application.Features.Accounts.Commands.CloseAccount;
using DigitalBanking.Application.Features.Accounts.Commands.CreateAccount;
using DigitalBanking.Application.Features.Accounts.Commands.FreezeAccount;
using DigitalBanking.Application.Features.Accounts.DTOs;
using DigitalBanking.Application.Features.Accounts.Queries.GetAccountById;
using DigitalBanking.Application.Features.Accounts.Queries.SearchAccounts;
using DigitalBanking.WebAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBanking.WebAPI.Controllers
{
    [ApiController]
    [ApiVersion(1)]
    [Route("api/v{version:apiVersion}/accounts")]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public AccountsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{accountId:guid}")]
        public async Task<IActionResult> GetAccountsByIdAsync([FromRoute] Guid accountId, CancellationToken cancellationToken)
        {
            var request = new GetAccountByIdQuery { AccountId = accountId };
            var result = await _mediator.Send(request, cancellationToken);

            return Ok(new ApiResponse<AccountDto>
            {
                Data = result,
                Message = "Account retrieved successfully"
            });
        }

        [HttpGet]
        public async Task<IActionResult> SearchAccountsAsync([FromQuery] SearchAccountsQuery request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(new ApiResponse<List<AccountDto>>
            {
                Data = result,
                Message = "Retrieved account list with given search criteria"
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccountAsync([FromBody] CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            return CreatedAtAction(
                nameof(CreateAccountAsync),
                new { accountId = result },
                new ApiResponse<Guid>
                {
                    Data = result,
                    Message = "Account created successfully"
                });
        }

        [HttpPost("{accountId:guid}/activate")]
        public async Task<IActionResult> ActivateAccountAsync([FromRoute] Guid accountId, CancellationToken cancellationToken)
        {
            var request = new ActivateAccountCommand { AccountId = accountId };
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }

        [HttpPost("{accountId:guid}/freeze")]
        public async Task<IActionResult> FreezeAccountAsync([FromRoute] Guid accountId, CancellationToken cancellationToken)
        {
            var request = new FreezeAccountCommand { AccountId = accountId };
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }

        [HttpPost("{accountId:guid}/close")]
        public async Task<IActionResult> CloseAccountAsync([FromRoute] Guid accountId, CancellationToken cancellationToken)
        {
            var request = new CloseAccountCommand { AccountId = accountId };
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }
    }
}
