using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace DigitalBanking.WebAPI.Controllers
{
    // Global rate limitter

    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetProfile()
        {
            var userId = User.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var role = User.FindFirstValue(ClaimTypes.Role);
            var permissions = User.FindFirstValue("permission");

            return Ok(new { userId, role, permissions });
        }
    }
}
