using Asp.Versioning;
using DigitalBanking.Application.Features.Customers.DTOs;
using DigitalBanking.Application.Features.Customers.SearchCustomers.Queries;
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

        [HttpGet("search")]
        public async Task<IActionResult> SearchCustomerAsync([FromQuery] SearchCustomerQuery request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(new ApiResponse<List<Customer>>
            {
                Data = response,
                Message = "Customer found."
            });
        }
    }
}
