using Asp.Versioning;
using DigitalBanking.Application.Features.Transfers.Commands;
using DigitalBanking.Application.Features.Transfers.DTOs;
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
    [Route("api/v{version:apiVersion}/transfer")]
    [Authorize]
    public class TransferFundsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransferFundsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> TransferFunds([FromBody] TransferFundsCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(new ApiResponse<TransferFundsDto>
            {
                Data = response,
                Message = "Funds transferred successfully"
            });
        }
    }
}
