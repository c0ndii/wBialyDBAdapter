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

        public async Task<IEnumerable<UserGetDto>> GetAll(int excludedUserId)
        {
            return await _context.Users
                .Where(x => x.UserId != excludedUserId)
                .Select(x => new UserGetDto { Id = x.UserId, Username = x.Username })
                .ToListAsync();
        }

        public async Task<UserGetDto?> GetAsync(int userId)
        {
            return await _context.Users
                .Where(x => x.UserId == userId)
                .Select(x => new UserGetDto { Id = x.UserId, Username = x.Username })
                .FirstOrDefaultAsync();
        }

        public async Task Register(UserRegisterInput input)
        {
            var user = new Database.ObjectRelational.Entities.User.User
            {
                Login = input.Login,
                Password = input.Password,
                Username = input.Username,
            };

            await _context.Users.AddAsync(user);

            await _context.SaveChangesAsync();

            await _context.UserSecurityProfiles.AddAsync(new Database.ObjectRelational.Entities.User.UserSecurityProfile
            {
                UserId = user.UserId,
                IsLockoutEnabled = true,
                MaxFailedLoginAttempts = 5,
                IsPasswordManagerEnabled = true,
            });

            await _context.SaveChangesAsync();
        }
    }
}
