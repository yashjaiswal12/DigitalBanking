using DigitalBanking.Application.Features.Authentication.Commands.Login;
using DigitalBanking.Application.Features.Authentication.Commands.RegisterCustomer;
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
        public async Task<IActionResult> RegisterCustomer(RegisterCustomerCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginCustomer(LoginCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}
