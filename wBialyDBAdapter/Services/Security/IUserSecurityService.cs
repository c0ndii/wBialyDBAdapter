using wBialyDBAdapter.Model.User;

namespace wBialyDBAdapter.Services.Security
{
    public interface IUserSecurityService
    {
        Task<LoginChallengeDto> GetChallenge(string login);
        Task<LoginAttemptResultDto> VerifyPartialLogin(PartialLoginInput input, string? ipAddress, string? userAgent);

        Task<bool> UpdatePartialPasswordSlot(int userId, string masterPassword, string newPartialPassword, int slotIndex);

        Task<UserSecurityOverviewDto> GetSecurityOverview(int userId);
        Task<UserSecuritySettingsDto> UpdateSecuritySettings(int userId, UserSecuritySettingsDto input);
        Task<IEnumerable<LoginAuditItemDto>> GetLoginAudits(int currentUserId, int limit, LogsScope scope);
        Task<bool> ChangeMasterPassword(int userId, string oldPassword, string newPassword);
    }
}