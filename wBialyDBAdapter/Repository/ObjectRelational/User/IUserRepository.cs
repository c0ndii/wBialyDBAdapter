using wBialyDBAdapter.Model.User;

namespace wBialyDBAdapter.Repository.ObjectRelational.User
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserGetDto>> GetAll(int exlucdedUserId);
        Task<UserGetDto?> GetAsync(int userId);
        Task Register(UserRegisterInput input);
    }
}
