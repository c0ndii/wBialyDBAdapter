using wBialyDBAdapter.Model.User;

namespace wBialyDBAdapter.Services.User
{
    public interface IUserService
    {
        Task<UserGetDto?> GetUserAsync(int userId);
        Task Register(UserRegisterInput input);
        Task<UserGetDto?> Login(UserLoginInput input);
    }
}
