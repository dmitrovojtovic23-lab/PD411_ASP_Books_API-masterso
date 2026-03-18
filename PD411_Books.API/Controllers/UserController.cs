using Microsoft.AspNetCore.Mvc;
using PD411_Books.API.Extensions;
using PD411_Books.BLL.Dtos.User;
using PD411_Books.BLL.Services;

namespace PD411_Books.API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IConfiguration _configuration;

        public UserController(UserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto)
        {
            var confirmationUrl = $"{Request.Scheme}://{Request.Host}/api/user/confirm-email";
            var response = await _userService.RegisterAsync(dto, confirmationUrl);
            return this.GetAction(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
        {
            var response = await _userService.LoginAsync(dto);
            return this.GetAction(response);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] string token)
        {
            var response = await _userService.ConfirmEmailAsync(token);
            return this.GetAction(response);
        }
    }
}
