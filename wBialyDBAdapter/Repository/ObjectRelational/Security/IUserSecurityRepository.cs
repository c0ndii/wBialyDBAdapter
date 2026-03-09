using wBialyDBAdapter.Model.User;

namespace wBialyDBAdapter.Repository.ObjectRelational.Security
{
    public interface IUserSecurityRepository
    {
        Task<LoginAttemptResultDto> Login(UserLoginInput input, string? ipAddress, string? userAgent);
        Task<UserSecurityOverviewDto> GetSecurityOverview(int userId);
        Task<UserSecuritySettingsDto> UpdateSecuritySettings(int userId, UserSecuritySettingsDto input);
    }
}
