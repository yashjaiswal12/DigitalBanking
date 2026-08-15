using Asp.Versioning;
using DigitalBanking.Application.Features.Authentication.Commands.Login;
using DigitalBanking.Application.Features.Authentication.Commands.Logout;
using DigitalBanking.Application.Features.Authentication.Commands.LogoutAllDevices;
using DigitalBanking.Application.Features.Authentication.Commands.RefreshToken;
using DigitalBanking.Application.Features.Authentication.Commands.RegisterCustomer;
using DigitalBanking.WebAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace DigitalBanking.WebAPI.Controllers
{
    [ApiController]
    [EnableRateLimiting("login-window")]
    [Authorize]
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
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

        [HttpPost("logout-all-devices")]
        public async Task<IActionResult> LogoutAllDevicesAsync(LogoutAllDevicesCommand request, CancellationToken cancellationToken)
        {
            await _mediator.Send(request, cancellationToken);
            return NoContent();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> PostRefreshToken(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(new ApiResponse<RefreshTokenResponse>
            {
                Data = response,
                Message = "Token refreshed successfully"
            });
        }
    }
}
