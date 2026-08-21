using Asp.Versioning;
using DigitalBanking.Application.Features.Statements.DTOs;
using DigitalBanking.Application.Features.Statements.Queries.GetStatement;
using DigitalBanking.WebAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBanking.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/accounts/{accountId:guid}")]
    public class StatementsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatementsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("statements")]
        public async Task<IActionResult> GetAccountStatements([FromRoute] Guid accountId, [FromBody] GenerateStatementRequestDto dto, 
            CancellationToken cancellationToken)
        {
            var query = new GetStatementQuery { AccountId = accountId, FromDateUtc = dto.FromDateUtc, ToDateUtc = dto.ToDateUtc };
            var result = await _mediator.Send(query, cancellationToken);
            
            return Ok(new ApiResponse<AccountStatementDto>
            {
                Data = result,
                Message = "Account statements retrieved successfully"
            });
        }
    }
}
