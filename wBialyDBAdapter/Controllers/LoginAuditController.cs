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
    public class LoginAuditController : ControllerBase
    {
        private readonly IUserSecurityService _userSecurityService;

        public LoginAuditController(IUserSecurityService userSecurityService)
        {
            _userSecurityService = userSecurityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs(
            [FromQuery] LogsScope scope = LogsScope.Mine,
            [FromQuery] int limit = 50)
        {
            return Ok(await _userSecurityService.GetLoginAudits(User.GetUserId(), limit, scope));
        }
    }
}
