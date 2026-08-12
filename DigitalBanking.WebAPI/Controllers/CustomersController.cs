using Asp.Versioning;
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

        [ProducesResponseType(typeof(ApiResponse<List<Customer>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [HttpGet("search")]
        public async Task<IActionResult> SearchCustomerAsync([FromQuery] SearchCustomerQuery request, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(request, cancellationToken);
            return Ok(new ApiResponse<List<Customer>>
            {
                Data = result,
                Message = "Customer found."
            });
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetCustomerByIdAsync([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new GetCustomerByIdQuery() { Id = id };
            var result = await _mediator.Send(request, cancellationToken);

            return Ok(new ApiResponse<Customer>(){
                Data = result,
                Message = "Customer retrieved successfully."
            });
        }

        [HttpPut("{id:Guid}")]
        public async Task<IActionResult> UpdateCustomerAsync([FromRoute] Guid id, UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            request.CustomerId = id;
            var result = await _mediator.Send(request, cancellationToken);

            return Ok(new ApiResponse<Customer>
            {
                Data = result,
                Message = "Customer information updated successfully."
            });
        }

        [HttpDelete("{id:Guid}")]
        public async Task<IActionResult> DeactivateCustomer([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var request = new DeactivateCustomerCommand { CustomerId = id };
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }
    }
}
