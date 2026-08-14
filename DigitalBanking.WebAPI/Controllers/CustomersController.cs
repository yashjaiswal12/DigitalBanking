using Asp.Versioning;
using DigitalBanking.Application.Authorization;
using DigitalBanking.Application.Features.Customers.Commands.DeactivateCustomer;
using DigitalBanking.Application.Features.Customers.Commands.UpdateCustomer;
using DigitalBanking.Application.Features.Customers.DTOs;
using DigitalBanking.Application.Features.Customers.Queries.GetCustomerById;
using DigitalBanking.Application.Features.Customers.Queries.SearchCustomers;
using DigitalBanking.WebAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBanking.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CustomersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [ProducesResponseType(typeof(ApiResponse<List<CustomerDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Policy = Permissions.ViewCustomers)]
        [HttpGet("search")]
        public async Task<IActionResult> SearchCustomerAsync([FromQuery] SearchCustomerQuery request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(new ApiResponse<List<CustomerDto>>
            {
                Data = result,
                Message = "Customer found."
            });
        }

        [Authorize(Policy = Permissions.ViewCustomers)]
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetCustomerByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new GetCustomerByIdQuery() { Id = id };
            var result = await _mediator.Send(request, cancellationToken);

            return Ok(new ApiResponse<CustomerDto>(){
                Data = result,
                Message = "Customer retrieved successfully."
            });
        }

        [Authorize(Policy = Permissions.ManageCustomers)]
        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateCustomerAsync([FromRoute] Guid id, UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            request.CustomerId = id;
            var result = await _mediator.Send(request, cancellationToken);

            return Ok(new ApiResponse<CustomerDto>
            {
                Data = result,
                Message = "Customer information updated successfully."
            });
        }

        [Authorize(Policy = Permissions.ManageCustomers)]
        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeactivateCustomer([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new DeactivateCustomerCommand { CustomerId = id };
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }
    }
}
