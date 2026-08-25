using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace DigitalBanking.WebAPI.Controllers
{
    [Authorize]
    [EnableRateLimiting("request-limit")]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/customers/{customerId:guid}/kyc-documents")]
    public class KycDocumentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public KycDocumentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{documentId:string}")]
        public async Task<IActionResult> GetKycDocument([FromRoute] Guid customerId, [FromRoute] string documentId, 
            CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UploadKycDocument([FromRoute] Guid customerId, CancellationToken cancellationToken)
        {
            return Ok();
        }

        [HttpDelete("{documentId:string}")]
        public async Task<IActionResult> DeleteKycDocument([FromRoute] Guid customerId, [FromRoute] string documentId,
            CancellationToken cancellationToken)
        {
            return NoContent();
        }

        [HttpPost("{documentId:string}/access-url")]
        public async Task<IActionResult> GetAccessUrl([FromRoute] Guid customerId, [FromRoute] string documentId,
            CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}
