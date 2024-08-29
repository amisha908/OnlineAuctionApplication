using Microsoft.AspNetCore.Mvc;
using OnlineAuction.DAL.Model.Domain;
using OnlineAuction.BLL.Services.Interface;
using System.Threading.Tasks;
using OnlineAuction.DAL.Model.Dto;

namespace OnlineAuction.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _authService.AuthenticateUserAsync(model.Email, model.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var token = await _authService.GenerateJwtTokenAsync(user);
            return Ok(new { Token = token });
        }
        [HttpGet("username/{userId}")]
        public async Task<IActionResult> GetUsernameById(string userId)
        {
            var username = await _authService.GetUsernameByIdAsync(userId);
            if (username == null)
            {
                return NotFound("User not found.");
            }

            return Ok(new { Username = username });
        }
    }

    
}

