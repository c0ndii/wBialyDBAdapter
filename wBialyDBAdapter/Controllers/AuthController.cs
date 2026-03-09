using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using wBialyDBAdapter.Model.User;
using wBialyDBAdapter.Services.Auth;

namespace wBialyDBAdapter.Controllers
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
        public async Task<IActionResult> Login([FromBody] UserLoginInput input)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = HttpContext.Request.Headers.UserAgent.ToString();

            var result = await _authService.Login(input, ipAddress, userAgent);
            if (!result.IsSuccess || result.User == null)
            {
                if (result.RetryAfterSeconds.HasValue)
                {
                    HttpContext.Response.Headers.RetryAfter = result.RetryAfterSeconds.Value.ToString();
                }

                return Unauthorized(new
                {
                    message = result.Message,
                    retryAfterSeconds = result.RetryAfterSeconds,
                });
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, result.User.Username),
                new Claim(ClaimTypes.NameIdentifier, result.User.Id.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");

            await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity));

            return Ok();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterInput input)
        {
            await _authService.Register(input);
            return Ok();
        }
    }
}
