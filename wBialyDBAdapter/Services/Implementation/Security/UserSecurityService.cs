using wBialyDBAdapter.Model.User;
using wBialyDBAdapter.Repository.ObjectRelational.Security;
using wBialyDBAdapter.Services.Security;

namespace wBialyDBAdapter.Services.Implementation.Security
{
    public class UserSecurityService : IUserSecurityService
    {
        private readonly IUserSecurityRepository _userSecurityRepository;
        private readonly ILoginAuditRepository _loginAuditRepository;

        public UserSecurityService(
            IUserSecurityRepository userSecurityRepository,
            ILoginAuditRepository loginAuditRepository)
        {
            _userSecurityRepository = userSecurityRepository;
            _loginAuditRepository = loginAuditRepository;
        }

        public async Task<LoginChallengeDto> GetChallenge(string login)
        {
            return await _userSecurityRepository.GetChallenge(login);
        }

        public async Task<LoginAttemptResultDto> VerifyPartialLogin(PartialLoginInput input, string? ipAddress, string? userAgent)
        {
            return await _userSecurityRepository.VerifyPartialLogin(input, ipAddress, userAgent);
        }

        public async Task<bool> UpdatePartialPasswordSlot(int userId, string masterPassword, string newPartialPassword, int slotIndex)
        {
            return await _userSecurityRepository.UpdatePartialPasswordSlot(userId, masterPassword, newPartialPassword, slotIndex);
        }

        public async Task<UserSecurityOverviewDto> GetSecurityOverview(int userId)
        {
            return await _userSecurityRepository.GetSecurityOverview(userId);
        }

        public async Task<UserSecuritySettingsDto> UpdateSecuritySettings(int userId, UserSecuritySettingsDto input)
        {
            return await _userSecurityRepository.UpdateSecuritySettings(userId, input);
        }

        public async Task<IEnumerable<LoginAuditItemDto>> GetLoginAudits(int currentUserId, int limit, LogsScope scope)
        {
            if (scope == LogsScope.Mine)
            {
                return await _loginAuditRepository.GetLoginAudits(currentUserId, false, limit);
            }

            return await _loginAuditRepository.GetLoginAudits(null, true, limit);
        }

        public async Task<bool> ChangeMasterPassword(int userId, string oldPassword, string newPassword)
        {
            return await _userSecurityRepository.ChangeMasterPassword(userId, oldPassword, newPassword);
        }
    }
}