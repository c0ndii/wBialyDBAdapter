using wBialyDBAdapter.Model.User;
using wBialyDBAdapter.Repository.ObjectRelational.User;
using wBialyDBAdapter.Services.User;

namespace wBialyDBAdapter.Services.Implementation.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserGetDto?> GetUserAsync(int userId)
        {
            return await _userRepository.GetAsync(userId);
        }

        public async Task Register(UserRegisterInput input)
        {
            await _userRepository.Register(input);
        }

        public async Task<bool> Login(UserLoginInput input)
        {
            return await _userRepository.Login(input);
        }
    }
}
