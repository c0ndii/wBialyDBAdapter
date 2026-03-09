using wBialyDBAdapter.Model.User;
using wBialyDBAdapter.Repository.ObjectRelational.User;
using wBialyDBAdapter.Services.Auth;
using wBialyDBAdapter.Services.Security;

namespace wBialyDBAdapter.Services.Implementation.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserSecurityService _userSecurityService;

        public AuthService(IUserRepository userRepository, IUserSecurityService userSecurityService)
        {
            _userRepository = userRepository;
            _userSecurityService = userSecurityService;
        }

        public async Task Register(UserRegisterInput input)
        {
            await _userRepository.Register(input);
        }

        public async Task<LoginAttemptResultDto> Login(UserLoginInput input, string? ipAddress, string? userAgent)
        {
            return await _userSecurityService.Login(input, ipAddress, userAgent);
        }
    }
}
