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

        public async Task<LoginChallengeDto> GetChallenge(string login)
        {
            return await _userSecurityService.GetChallenge(login);
        }

        public async Task<LoginAttemptResultDto> VerifyPartialLogin(PartialLoginInput input, string? ipAddress, string? userAgent)
        {
            return await _userSecurityService.VerifyPartialLogin(input, ipAddress, userAgent);
        }

        public async Task<bool> UpdatePartialPassword(int userId, string masterPassword, string newPartialPassword, int slotIndex)
        {
            return await _userSecurityService.UpdatePartialPasswordSlot(userId, masterPassword, newPartialPassword, slotIndex);
        }

        public async Task<bool> ChangeMasterPassword(int userId, string oldPassword, string newPassword)
        {
            return await _userSecurityService.ChangeMasterPassword(userId, oldPassword, newPassword);
        }
    }
}