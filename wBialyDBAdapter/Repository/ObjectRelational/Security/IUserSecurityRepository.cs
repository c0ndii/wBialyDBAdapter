using wBialyDBAdapter.Model.User;

namespace wBialyDBAdapter.Repository.ObjectRelational.Security
{
    public interface IUserSecurityRepository
    {
        Task<UserSecurityOverviewDto> GetSecurityOverview(int userId);
        Task<UserSecuritySettingsDto> UpdateSecuritySettings(int userId, UserSecuritySettingsDto input);
        Task<LoginChallengeDto> GetChallenge(string login);
        Task<LoginAttemptResultDto> VerifyPartialLogin(PartialLoginInput input, string? ipAddress, string? userAgent);
        Task<bool> UpdatePartialPasswordSlot(int userId, string masterPassword, string newPartialPassword, int slotIndex);
        Task<bool> ChangeMasterPassword(int userId, string oldPassword, string newPassword);
    }
}