using wBialyDBAdapter.Model.User;

namespace wBialyDBAdapter.Services.User
{
    public interface IUserService
    {
        Task<IEnumerable<UserGetDto>> GetUsers(int excludeUserId);
        Task<UserGetDto?> GetUserAsync(int userId);
        Task Register(UserRegisterInput input);
        Task<UserGetDto?> Login(UserLoginInput input);
    }
}
