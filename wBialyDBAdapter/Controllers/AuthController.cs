using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("login/challenge/{login}")]
        public async Task<IActionResult> GetChallenge(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                return BadRequest(new { message = "Login nie mo¿e byæ pusty." });

            try
            {
                var challenge = await _authService.GetChallenge(login);
                return Ok(challenge);
            }
            catch (Exception)
            {
                return Unauthorized(new { message = "Logowanie niemo¿liwe." });
            }
        }

        [HttpPost("login/verify")]
        public async Task<IActionResult> VerifyPartial([FromBody] PartialLoginInput input)
        {
            if (input == null || string.IsNullOrWhiteSpace(input.Login) || input.ProvidedCharacters == null || !input.ProvidedCharacters.Any())
            {
                return BadRequest(new { message = "Nieprawid³owe dane logowania. Podaj znaki has³a." });
            }

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = HttpContext.Request.Headers.UserAgent.ToString();

            var result = await _authService.VerifyPartialLogin(input, ipAddress, userAgent);

            if (!result.IsSuccess || result.User == null)
            {
                if (result.RetryAfterSeconds.HasValue)
                {
                    HttpContext.Response.Headers.RetryAfter = result.RetryAfterSeconds.Value.ToString();
                }

                return Unauthorized(new
                {
                    message = result.Message ?? "B³êdne znaki has³a.",
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

            return Ok(new { message = "Zalogowano pomyœlnie. Slot zosta³ zrotowany." });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterInput input)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (input.Password.Length < 12 || input.Password.Length > 18)
                return BadRequest(new { message = "Has³o g³ówne musi mieæ od 12 do 18 znaków." });

            try
            {
                await _authService.Register(input);
                return Ok(new { message = "Zarejestrowano i wygenerowano 10 wyzwañ." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("password/change-master")]
        public async Task<IActionResult> ChangeMasterPassword([FromBody] ChangeMasterPasswordInput input)
        {
            if (input == null || string.IsNullOrWhiteSpace(input.OldPassword) || string.IsNullOrWhiteSpace(input.NewPassword))
                return BadRequest(new { message = "Podaj stare i nowe has³o." });

            if (input.NewPassword.Length < 12 || input.NewPassword.Length > 18)
                return BadRequest(new { message = "Nowe has³o musi mieæ od 12 do 18 znaków." });

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var success = await _authService.ChangeMasterPassword(userId, input.OldPassword, input.NewPassword);

            if (!success)
                return BadRequest(new { message = "Zmiana has³a nieudana. SprawdŸ czy stare has³o jest poprawne." });

            return Ok(new { message = "Has³o g³ówne zosta³o zmienione. Wygenerowano 10 nowych wyzwañ." });
        }

        public class ChangeMasterPasswordInput
        {
            public string OldPassword { get; set; } = string.Empty;
            public string NewPassword { get; set; } = string.Empty;
        }
    }

    public class UpdateSlotInput
    {
        public string MasterPassword { get; set; } = string.Empty;
        public string NewPartialPassword { get; set; } = string.Empty;
        public int SlotIndex { get; set; }
    }
}