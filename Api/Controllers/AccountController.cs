using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public record ClientRegisterRequest(string Email, string Password);

       
        [HttpPost("register")]
        public async Task<IActionResult> Register(ClientRegisterRequest request)
        {
            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors.Select(e => e.Description));

            await _userManager.AddToRoleAsync(user, "Client");
            return Ok();
        }

       
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> Me()
        {
            var user = await _userManager.FindByIdAsync(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (user is null)
                return Unauthorized();

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new { id = user.Id, email = user.Email, roles });
        }
    }
}