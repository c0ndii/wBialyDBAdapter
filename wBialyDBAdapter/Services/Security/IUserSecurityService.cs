using wBialyDBAdapter.Model.User;

namespace wBialyDBAdapter.Services.Security
{
    public interface IUserSecurityService
    {
        Task<LoginAttemptResultDto> Login(UserLoginInput input, string? ipAddress, string? userAgent);
        Task<UserSecurityOverviewDto> GetSecurityOverview(int userId);
        Task<UserSecuritySettingsDto> UpdateSecuritySettings(int userId, UserSecuritySettingsDto input);
        Task<IEnumerable<LoginAuditItemDto>> GetLoginAudits(int currentUserId, int limit, LogsScope scope);
    }
}
