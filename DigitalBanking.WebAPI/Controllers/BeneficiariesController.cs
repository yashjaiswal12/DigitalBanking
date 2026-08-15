using Asp.Versioning;
using DigitalBanking.Application.Authorization;
using DigitalBanking.Application.Features.Beneficiaries.Commands.AddBeneficiary;
using DigitalBanking.Application.Features.Beneficiaries.Commands.RemoveBeneficiary;
using DigitalBanking.Application.Features.Beneficiaries.DTOs;
using DigitalBanking.Application.Features.Beneficiaries.Queries.GetBeneficiaries;
using DigitalBanking.Application.Features.Beneficiaries.Queries.GetBeneficiaryById;
using DigitalBanking.WebAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace DigitalBanking.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/beneficiaries")]
    [EnableRateLimiting("request-limit")]
    public class BeneficiariesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BeneficiariesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Policy = Permissions.ManageCustomers)]
        public async Task<IActionResult> AddBeneficiary([FromBody] AddBeneficiaryCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(new ApiResponse<Guid>
            {
                Data = response,
                Message = "Beneficiary Added Successfully"
            });
        }

        [HttpGet]
        [Authorize(Policy = Permissions.ViewCustomers)]
        public async Task<IActionResult> GetBeneficiaries(GetBeneficiariesQuery request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(new ApiResponse<List<BeneficiaryDto>>
            {
                Data = response,
                Message = "Beneficiary List Retrieved Successfully"
            });
        }

        [HttpGet("{id:guid}")]
        [Authorize(Policy = Permissions.ViewCustomers)]
        public async Task<IActionResult> GetBeneficiaryById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new GetBeneficiaryByIdQuery { BeneficiaryId = id };
            var response = await _mediator.Send(request, cancellationToken);

            return Ok(new ApiResponse<BeneficiaryDto>
            {
                Data = response,
                Message = "Beneficiary Retrieved Successfully"
            });
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = Permissions.ManageCustomers)]
        public async Task<IActionResult> DeleteBeneficiary([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new RemoveBeneficiaryCommand { BeneficiaryId = id };
            await _mediator.Send(request, cancellationToken);

            return NoContent();
        }
    }
}
