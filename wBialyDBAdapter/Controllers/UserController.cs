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

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            return Ok(await _userService.GetUserAsync(id));
        }

        [HttpGet("me")]
        [Authorize]
        public IActionResult GetCurrentUser()
        {
            var id = HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var username = HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

            return Ok(new UserGetDto
            {
                Id = int.Parse(id),
                Username = username
            });
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return Ok();
        }

    }
}
