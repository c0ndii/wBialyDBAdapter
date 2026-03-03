using wBialyDBAdapter.Model.User;

namespace wBialyDBAdapter.Repository.ObjectRelational.User
{
    public interface IUserRepository
    {
        Task<UserGetDto?> GetAsync(int userId);
        Task Register(UserRegisterInput input);
        Task<UserGetDto?> Login(UserLoginInput input);
    }
}
