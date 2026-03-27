using wBialyDBAdapter.Model.User;

namespace wBialyDBAdapter.Services.Auth
{
    public interface IAuthService
    {
        Task Register(UserRegisterInput input);

        Task<LoginChallengeDto> GetChallenge(string login);

        Task<LoginAttemptResultDto> VerifyPartialLogin(PartialLoginInput input, string? ipAddress, string? userAgent);

        Task<bool> UpdatePartialPassword(int userId, string masterPassword, string newPartialPassword, int slotIndex);
        Task<bool> ChangeMasterPassword(int userId, string oldPassword, string newPassword);
    }
}