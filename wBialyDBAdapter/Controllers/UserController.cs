using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using wBialyDBAdapter.Extensions;
using wBialyDBAdapter.Model.User;
using wBialyDBAdapter.Services.User;

namespace wBialyDBAdapter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.GetUsers(User.GetUserId()));
        }

        [HttpGet("/{id}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _userService.GetUserAsync(id));
        }

        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] UserLoginInput input)
        {
            var result = await _userService.Login(input);
            if (result == null)
                return Unauthorized();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, result.Username),
                new Claim(ClaimTypes.NameIdentifier, result.Id.ToString())
            };
            var claimsIdentity = new ClaimsIdentity(claims, "MyCookieAuth");

            await HttpContext.SignInAsync("MyCookieAuth", new ClaimsPrincipal(claimsIdentity));

            return Ok();
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterInput input)
        {
            await _userService.Register(input);
            return Ok();
        }
    }
}
