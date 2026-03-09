using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using wBialyDBAdapter.Extensions;
using wBialyDBAdapter.Model.User;
using wBialyDBAdapter.Services.Security;

namespace wBialyDBAdapter.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserSecurityController : ControllerBase
    {
        private readonly IUserSecurityService _userSecurityService;

        public UserSecurityController(IUserSecurityService userSecurityService)
        {
            _userSecurityService = userSecurityService;
        }

        [HttpGet("overview")]
        public async Task<IActionResult> GetOverview()
        {
            return Ok(await _userSecurityService.GetSecurityOverview(User.GetUserId()));
        }

        [HttpPut("settings")]
        public async Task<IActionResult> UpdateSettings([FromBody] UserSecuritySettingsDto input)
        {
            return Ok(await _userSecurityService.UpdateSecuritySettings(User.GetUserId(), input));
        }
    }
}
