using Asp.Versioning;
using DigitalBanking.Application.Common.Pagination;
using DigitalBanking.Application.Features.Transfers.Commands;
using DigitalBanking.Application.Features.Transfers.DTOs;
using DigitalBanking.Application.Features.Transfers.Queries.GetTransactionById;
using DigitalBanking.Application.Features.Transfers.Queries.GetTransactionHistory;
using DigitalBanking.WebAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace DigitalBanking.WebAPI.Controllers
{
    [ApiController]
    [ApiVersion("1")]
    //[EnableRateLimiting("")]
    [Route("api/v{version:apiVersion}/accounts")]
    [Authorize]
    public class TransferFundsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransferFundsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> TransferFunds([FromBody] TransferFundsCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(new ApiResponse<TransferFundsDto>
            {
                Data = response,
                Message = "Funds transferred successfully"
            });
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> GetTransactionHistoryAsync([FromQuery] GetTransactionHistoryQuery request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);

            return Ok(new ApiResponse<PagedResult<TransactionHistoryDto>>
            {
                Data = result,
                Message = "Transaction history retrieved successfully"
            });
        }

        [HttpGet("transactions/{transactionId:guid}")]
        public async Task<IActionResult> GetTransactionHistoryAsync([FromRoute] Guid transactionId, CancellationToken cancellationToken)
        {
            var request = new GetTransactionByIdQuery { TransactionId = transactionId };
            var result = await _mediator.Send(request, cancellationToken);

            return Ok(new ApiResponse<TransactionDetailDto>
            {
                Data = result,
                Message = "Transaction details retrieved successfully"
            });
        }
    }
}
