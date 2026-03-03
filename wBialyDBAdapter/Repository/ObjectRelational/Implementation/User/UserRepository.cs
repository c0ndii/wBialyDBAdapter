using Microsoft.EntityFrameworkCore;
using wBialyDBAdapter.Database.ObjectRelational;
using wBialyDBAdapter.Model.User;
using wBialyDBAdapter.Repository.ObjectRelational.User;

namespace wBialyDBAdapter.Repository.ObjectRelational.Implementation.User
{
    public class UserRepository : IUserRepository
    {
        private readonly ORDB _context;
        public UserRepository(ORDB context)
        {
            _context = context;
        }

        public async Task<UserGetDto?> GetAsync(int userId)
        {
            return await _context.Users
                .Where(x => x.UserId == userId)
                .Select(x => new UserGetDto { Id = x.UserId, Username = x.Username, Password = x.Password, Login = x.Login })
                .FirstOrDefaultAsync();
        }

        public async Task Register(UserRegisterInput input)
        {
            await _context.Users.AddAsync(new Database.ObjectRelational.Entities.User.User
            {
                Login = input.Login,
                Password = input.Password,
                Username = input.Username,
            });
        }

        public async Task<bool> Login(UserLoginInput input)
        {
            return 
                (await _context.Users.FirstOrDefaultAsync(x => x.Login == input.Login && x.Password == input.Password)) != null
                ? true : false;
        }
    }
}
