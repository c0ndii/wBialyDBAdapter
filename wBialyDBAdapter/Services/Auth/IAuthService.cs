using wBialyDBAdapter.Model.User;

namespace wBialyDBAdapter.Services.Auth
{
    public interface IAuthService
    {
        Task Register(UserRegisterInput input);
        Task<LoginAttemptResultDto> Login(UserLoginInput input, string? ipAddress, string? userAgent);
    }
}
