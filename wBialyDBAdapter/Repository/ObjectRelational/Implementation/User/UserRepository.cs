using Microsoft.EntityFrameworkCore;
using wBialyDBAdapter.Database.ObjectRelational;
using wBialyDBAdapter.Model.User;

namespace wBialyDBAdapter.Repository.ObjectRelational.Implementation.User
{
    public class UserRepository
    {
        private readonly ORDB _context;
        public UserRepository(ORDB context)
        {
            _context = context;
        }

        public async Task<Database.ObjectRelational.Entities.User.User?> GetUserAsync(int userId)
        {
            return (await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId)) ?? null;
        }

        public async Task<UserRegisterInput> 
    }
}
