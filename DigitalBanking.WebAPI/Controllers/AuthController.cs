using DigitalBanking.Application.Features.Authentication.Commands.Login;
using DigitalBanking.Application.Features.Authentication.Commands.Logout;
using DigitalBanking.Application.Features.Authentication.Commands.RegisterCustomer;
using DigitalBanking.WebAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DigitalBanking.WebAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(new ApiResponse<RegisterCustomerResponse>
            {
                Message = "Customer registered successfully",
                Data = response
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginCustomer([FromBody] LoginCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(new ApiResponse<LoginResponse>
            {
                Message = "Login Sucessful",
                Data = response
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> LogoutCustomer([FromBody] LogoutCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }
    }
}
